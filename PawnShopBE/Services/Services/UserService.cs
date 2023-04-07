using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Helpers;
using PawnShopBE.Infrastructure.Helpers;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class UserService : IUserService
    {
        public IUnitOfWork _unitOfWork;
        //private readonly string _gmail = "nguyentuanvu020901@gmail.com";
        //private readonly string _pass = "fhnwtwqisekdqzcr";
        private readonly string _gmail= "hethongpawns@gmail.com";
        private readonly string _pass = "1234abcd*";
        private IMapper _mapper;
        private DbContextClass _dbContextClass;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper, DbContextClass dbContextClass)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _dbContextClass = dbContextClass;
        }
        public async Task<bool> CreateUser(User user)
        {
            if (user != null)
            {
                user.CreateTime = DateTime.Now;
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                await _unitOfWork.Users.Add(user);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<bool> CreateAdmin(Admin admin)
        {
            if (admin != null)
            {
                admin.Password = BCrypt.Net.BCrypt.HashPassword(admin.Password);  
                
                _dbContextClass.Admin.Add(admin);
                var result = _dbContextClass.SaveChanges();           
                return (result > 0) ? true: false;       
            }
            return false;
        }
        public async Task<bool> sendEmail(UserDTO userDTO)
        {
            string sendto = userDTO.Email;
            string subject = "[PAWNSHOP] - Khôi phục mật khẩu" ;
            string content = "Mật khẩu mới cho tài khoản đăng nhập" + userDTO.FullName ;

            // Create random password
            string randomPassword = HelperFuncs.GeneratePassword(10);
            //set new password
            userDTO.Password = randomPassword;
            //update password
            var user= _mapper.Map<User>(userDTO);
            await UpdateUser(user);
            try
            {
                MailMessage mail=new MailMessage();
                SmtpClient smtp=new SmtpClient("smtp.gmail.com");
                //set property for email you want to send
                mail.From = new MailAddress(_gmail);
                mail.To.Add(sendto);
                mail.Subject = subject;
                mail.IsBodyHtml= true;
                mail.Body = content;
                mail.Priority=MailPriority.High;
                //set smtp port
                smtp.Port = 587;
                smtp.UseDefaultCredentials= false;
                //set gmail pass sender
                smtp.Credentials= new NetworkCredential(_gmail, _pass);
                smtp.EnableSsl = true;
                smtp.Send(mail);

                return true;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        public async Task<bool> DeleteUser(Guid userId)
        {
            if (userId != null)
            {
                var user = await _unitOfWork.Users.GetById(userId);
                if (user != null)
                {
                    _unitOfWork.Users.Delete(user);
                    var result = _unitOfWork.Save();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }

        public async Task<IEnumerable<User>> GetAllUsers(int num)
        {
            var userList = await _unitOfWork.Users.GetAll();
            if (num == 0)
            {
                return userList;
            }
            var result = await _unitOfWork.Users.TakePage(num, userList);
            return result;
        }

        public async Task<User> GetUserById(Guid userId)
        {
            if (userId != null)
            {
                var user = await _unitOfWork.Users.GetById(userId);
                if (user != null)
                {
                    return user;
                }
            }
            return null;
        }

        public async Task<bool> UpdateUser(User user)
        {
            if (user != null)
            {
                var userUpdate = await _unitOfWork.Users.GetById(user.UserId);
                if (userUpdate != null)
                {
                    userUpdate.UserName = user.UserName;
                    userUpdate.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    userUpdate.Status = user.Status;
                    userUpdate.Email= user.Email;
                    userUpdate.Phone = user.Phone;
                    userUpdate.Address= user.Address;
                    userUpdate.FullName= user.FullName;
                    userUpdate.Role= user.Role;
                    userUpdate.Branch= user.Branch;
                    userUpdate.UpdateTime = DateTime.Now;
                    _unitOfWork.Users.Update(userUpdate);

                    var result = _unitOfWork.Save();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
    }
}
