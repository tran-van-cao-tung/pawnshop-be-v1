using PawnShopBE.Core.Display;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class UserService : IUserService
    {
        public IUnitOfWork _unitOfWork;
        private readonly IBranchService _branchService;

        public UserService(IUnitOfWork unitOfWork, IBranchService branchService)
        {
            _unitOfWork = unitOfWork;
            _branchService = branchService;
        }
        public async Task<bool> CreateUser(User user)
        {
            if (user != null)
            {
                user.CreateTime = DateTime.Now;
                await _unitOfWork.Users.Add(user);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
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

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var userList = await _unitOfWork.Users.GetAll();
            return userList;
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

        public async Task<IEnumerable<DisplayUser>> getUserDTO(IEnumerable<DisplayUser> userDTOList,
            IEnumerable<User> userList)
        {
            int i = 1;
            foreach (var user in userDTOList)
            {
                var userId = user.BranchId;
                user.BranchName = await getBranchName(userId, userList);
                user.Num = i++;
            }
            return userDTOList;
        }

        private async Task<string> getBranchName(int userId, IEnumerable<User> userList)
        {
            //get branch list
            var branch = await _branchService.GetAllBranch();
            //get branch name
            var branchName = userList.Join(branch, u => u.BranchId, b => b.BranchId,
                (u, b) => { return b.BranchName; });
            var name = branchName.First().ToString();
            return name;
        }

    }
}
