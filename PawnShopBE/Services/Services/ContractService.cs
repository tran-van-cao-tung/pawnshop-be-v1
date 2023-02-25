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
    public class ContractService : IContractService
    {
        public IUnitOfWork _unitOfWork;

        public ContractService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateContract(Contract contract)
        {
            if (contract != null)
            {
                contract.ContractStartDate = DateTime.Now;
                await _unitOfWork.Contracts.Add(contract);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<bool> DeleteContract(int contractId)
        {
            if (contractId != null)
            {
                var contract = await _unitOfWork.Contracts.GetById(contractId);
                if (contract != null)
                {
                    _unitOfWork.Contracts.Delete(contract);
                    var result = _unitOfWork.Save();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }

        public async Task<IEnumerable<Contract>> GetAllContracts()
        {
            var contractList = await _unitOfWork.Contracts.GetAll();
            return contractList;
        }

        public async Task<Contract> GetContractById(int contractId)
        {
            if (contractId != null)
            {
                var contract = await _unitOfWork.Contracts.GetById(contractId);
                if (contract != null)
                {
                    return contract;
                }
            }
            return null;
        }

        public async Task<bool> UpdateContract(Contract contract)
        {
            if (contract != null)
            {
                var contractUpdate = await _unitOfWork.Contracts.GetById(contract.BranchId);
                if (contractUpdate != null)
                {
                    contractUpdate.Customer.FullName = contract.Customer.FullName;
                    contractUpdate.UpdateDate = DateTime.Now;
                    _unitOfWork.Contracts.Update(contractUpdate);

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
