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
    public class BranchService : IBranchService
    {

        public IUnitOfWork _unitOfWork;

        public BranchService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        
        public async Task<bool> CreateBranch(Branch branch)
        {
            if (branch != null)
            {
                branch.CreateDate = DateTime.Now;
                await _unitOfWork.Branches.Add(branch);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<bool> DeleteBranch(int branchId)
        {
            if (branchId != null)
            {
                var branch = await _unitOfWork.Branches.GetById(branchId);
                if (branch != null)
                {
                    _unitOfWork.Branches.Delete(branch);
                    var result = _unitOfWork.Save();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }

        public async Task<IEnumerable<Branch>> GetAllBranch()
        {
            var branchList = await _unitOfWork.Branches.GetAll();
            return branchList;
        }

        public async Task<Branch> GetBranchById(int branchId)
        {
            if (branchId != null)
            {
                var branch = await _unitOfWork.Branches.GetById(branchId);
                if (branch != null)
                {
                    return branch;
                }
            }
            return null;
        }

        public async Task<bool> UpdateBranch(Branch branch)
        {
            if (branch != null)
            {
                var branchUpdate = await _unitOfWork.Branches.GetById(branch.BranchId);
                if (branchUpdate != null)
                {
                    branchUpdate.BranchName = branch.BranchName;
                    branchUpdate.UpdateDate = DateTime.Now;
                    _unitOfWork.Branches.Update(branchUpdate);

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


       