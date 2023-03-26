using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PawnShopBE.Core.Display;

namespace Services.Services
{
    public class PermissionService: IPermissionService
    {
        private readonly IUnitOfWork _unit;
        private IBranchService _branch;
        private IUserService _user;

        public PermissionService(IUnitOfWork unitOfWork,IUserService user, IBranchService branch) { 
              _unit=unitOfWork;
            _user=user;
            _branch=branch;
        }
        public async Task SavePermission(IEnumerable<DisplayPermission> listPermission)
        {
            var listGroup = await _unit.UserPermissionGroup.GetAll();
            foreach(var p in listPermission)
            {
                UserPermissionGroup group= new UserPermissionGroup();
                group.UserId= p.UserId;
                group.perId = p.PermissionId;
                //check field đã tồn tại hay chưa
                var result = _unit.UserPermissionGroup.
                    SingleOrDefault(group,g => g.UserId==group.UserId && g.perId==group.perId);
                if (result == null)
                {
                    await _unit.UserPermissionGroup.Add(group);
                }
                else
                {
                     result.Status = p.Status;
                     _unit.UserPermissionGroup.Update(result);
                }
                 _unit.Save();
            }
        }

        public async Task<IEnumerable<DisplayPermission>> ShowPermission(UserPermissionDTO user)
        {
            //get list all
            var listPermission = await GetPermission();
            var listGroup = await _unit.UserPermissionGroup.GetAll();

            List<DisplayPermission> list = new List<DisplayPermission>();
            foreach(var p in listPermission)
            {
                DisplayPermission permission= new DisplayPermission();
                permission.PermissionId = p.perId;
                permission.UserId = user.UserId;
                permission.NameUser = user.NameUser;
                permission.NamePermission = p.description;
                permission.Status = getStatus(listGroup, p.perId, user.UserId);
                list.Add(permission);
            }
            return list;
        }

        private bool getStatus(IEnumerable<UserPermissionGroup> listGroup, int perId, Guid userId)
        {
            var resultIenumerable = from p in listGroup where p.perId == perId && p.UserId == userId select p;
            var result = resultIenumerable.FirstOrDefault();
            if (result!=null)
            {
                return result.Status;
            }
            return false;
        }

        public async Task<bool> CreatePermission(Permission permission)
        {
            var listPermisstion = await GetPermission();
            var checkPermission= from p in listPermisstion where p.description.CompareTo(permission.description) == 0 select p;

            if (checkPermission == null)
            {
                await _unit.Permission.Add(permission);
                var result = _unit.Save();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }


        public Task<bool> DeletePermission(int perId)
        {
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<Permission>> GetPermission()
        {
            var result = await _unit.Permission.GetAll();
            
            return result;
        }

        public Task<bool> UpdatePermission(Permission permission)
        {
            throw new NotImplementedException();
        }
    }
}
