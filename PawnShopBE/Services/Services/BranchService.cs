﻿using PawnShopBE.Core.Const;
using PawnShopBE.Core.Display;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Core.Requests;
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
        private IContractService _contract;
        private IInteresDiaryService _diary;
        private ILedgerService _ledgerService;
        private IServiceProvider _serviceProvider;

        public BranchService(IUnitOfWork unitOfWork, IContractService contract
           , ILedgerService ledger, IInteresDiaryService diary, IServiceProvider serviceProvider)
        {
            _unitOfWork = unitOfWork;
            _contract = contract;
            _ledgerService = ledger;
            _diary = diary;
            _serviceProvider = serviceProvider;
        }
        public async Task<DisplayBranchDetail> getDisplayBranchDetail(int branchId)
        {
            var branch = await _unitOfWork.Branches.GetById(branchId);

            if (branch == null)
            {
                return null;
            }
            // Get all ledger equal to branchId
            var ledgerList = await _ledgerService.GetLedgersByBranchId(branchId);         
            // Get all contract that not close and equal to branchId to get total loan
            var contractList = await _contract.GetAllContracts();
            var openContract = from c in contractList
                               where c.Status != (int)ContractConst.CLOSE && c.BranchId == branch.BranchId
                               select c; 

            var displayBranchDetail = new DisplayBranchDetail();
            displayBranchDetail.BranchId = branch.BranchId;
            displayBranchDetail.BranchName = branch.BranchName;
            displayBranchDetail.Loan = (long) openContract.Sum(c => c.Loan);
            displayBranchDetail.TotalContracts = contractList.Where(c => c.BranchId == branchId).Count();
            displayBranchDetail.OpenContract = openContract.Count();
            displayBranchDetail.CloseContract = displayBranchDetail.TotalContracts - displayBranchDetail.OpenContract;
            displayBranchDetail.Profit = (long) ledgerList.Sum(l => l.Profit);
            displayBranchDetail.CurrentFund =(long) (branch.Fund - openContract.Sum(c =>  c.Loan));
            return displayBranchDetail;
        }

        public async Task<DisplayBranchDetail> getDisplayBranchYearDetail(int branchId, int year)
        {
            var branch = await _unitOfWork.Branches.GetById(branchId);

            if (branch == null)
            {
                return null;
            }
            // Get all ledger equal to branchId and year
            var ledgerList = await _ledgerService.GetLedgersByBranchId(branchId, year);
            // Get all contract that not close and equal to branchId to get total loan
            var contractList = await _contract.GetAllContracts();
            var openContract = from c in contractList
                               where c.Status != (int)ContractConst.CLOSE && c.BranchId == branch.BranchId && c.ContractStartDate.Year == year
                               select c;

            var displayBranchDetail = new DisplayBranchDetail();
            displayBranchDetail.BranchId = branch.BranchId;
            displayBranchDetail.BranchName = branch.BranchName;
            displayBranchDetail.Loan = (long)openContract.Sum(c => c.Loan);
            displayBranchDetail.TotalContracts = contractList.Where(c => c.BranchId == branchId).Count();
            displayBranchDetail.OpenContract = openContract.Count();
            displayBranchDetail.CloseContract = displayBranchDetail.TotalContracts - displayBranchDetail.OpenContract;
            displayBranchDetail.Profit = (long)ledgerList.Sum(l => l.Profit);
            displayBranchDetail.CurrentFund = (long)(branch.Fund - openContract.Sum(c => c.Loan));
            return displayBranchDetail;
        }

        public async Task<IEnumerable<DisplayBranch>> getDisplayBranch()
        {
            var displayBranchList = new List<DisplayBranch>();
            var branchList = await GetAllBranch(0);
            foreach(var branch in branchList)
            {
                // Get all contract that not close and equal to branchid to get total loan
                var contractList = await _contract.GetAllContracts();
                var openContract = from c in contractList 
                                   where c.Status != (int)ContractConst.CLOSE && c.BranchId == branch.BranchId 
                                   select c;

                var displayBranch = new DisplayBranch();
                displayBranch.BranchId = branch.BranchId;
                displayBranch.BranchName = branch.BranchName;
                displayBranch.Address = branch.Address;
                displayBranch.PhoneNumber = branch.PhoneNumber;
                displayBranch.CurrentFund = branch.Fund - openContract.Sum(c => c.Loan);
                displayBranchList.Add(displayBranch);
            }           
            return displayBranchList;
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

        public async Task<IEnumerable<Branch>> GetAllBranch(int num)
        {
            var branchList = await _unitOfWork.Branches.GetAll();
            if (num == 0)
            {
                return branchList;
            }
            var result= await _unitOfWork.Branches.TakePage(num,branchList);
            return result;
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

        public async Task<bool> UpdateBranch(int id, BranchRequest branch)
        {
            if (branch != null)
            {
                var branchUpdate = await _unitOfWork.Branches.GetById(id);
                if (branchUpdate != null)
                {
                    branchUpdate.PhoneNumber = branch.PhoneNumber;
                    branchUpdate.Address = branch.Address;
                    branchUpdate.Fund = branch.Fund;
                    branchUpdate.BranchName = branch.BranchName;
                    branchUpdate.UpdateDate = DateTime.Now;
                    branchUpdate.Status = branch.Status;
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


       