using Azure;
using PawnShopBE.Core.Const;
using PawnShopBE.Core.Display;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class CustomerService : ICustomerService
    {
        public IUnitOfWork _unitOfWork;
        private readonly IContractService _contract;
        private readonly IBranchService _branch;
        public CustomerService(IUnitOfWork unitOfWork, IContractService contract,
           IBranchService branch) {
        _unitOfWork= unitOfWork;
            _branch = branch;
            _contract = contract;
        }
        public async Task<bool> CreateCustomer(Customer customer)
        {
            var oldCus = GetCustomerById(customer.CustomerId);

            if (customer != null) {

                customer.CreatedDate = DateTime.Now;
                await _unitOfWork.Customers.Add(customer);
                var result = _unitOfWork.Save();
                if(result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public async Task<bool> DeleteCustomer(Guid customerId)
        {
           if(customerId != null)
            {
                var customer= await _unitOfWork.Customers.GetById(customerId);
                if(customer != null)
                {
                    _unitOfWork.Customers.Delete(customer);
                    var result= _unitOfWork.Save();
                    if(result > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomer()
        {
            var listCustomer = await _unitOfWork.Customers.GetAll();
            if (listCustomer != null) 
            { 
            return listCustomer;
             }
            return null;
        }
        public async Task<IEnumerable<DisplayCustomer>> getCustomerHaveBranch(
            IEnumerable<DisplayCustomer> respone, IEnumerable<Customer> listCustomer)
        {
            int i = 1;
            foreach (var customer in respone)
            {
                // lấy customer id
                var customerId = customer.customerId;
                // lấy branchName
                customer.nameBranch = await GetBranchName(customerId, listCustomer);
                customer.numerical = i++;
            }
            return respone;
        }
        private async Task<string> GetBranchName(Guid customerId, IEnumerable<Customer> listCustomer)
        {
            //lấy danh sách contract
            var listContract = await _contract.GetAllContracts();
            // lấy branch id mà customer đang ở
            var branch = listCustomer.Join(listContract, p => p.CustomerId, c => c.CustomerId
                    , (p, c) => { return c.BranchId; });
            var branchtId = branch.First();
            // lấy danh sách branch
            var listBranch = await _branch.GetAllBranch();
            // lấy branchname
            var branchName = listContract.Join(listBranch, c => c.BranchId, b => b.BranchId,
                (c, b) => { return b.BranchName; });
            var name = branchName.First().ToString();
            return name;
        }
        public async Task<Customer> GetCustomerById(Guid idCus)
        {
            if (idCus != null)
            {
                var customer =await _unitOfWork.Customers.GetById(idCus);
                if(customer != null)
                {
                    return customer;
                }
            }
            return null;
        }

        public async Task<bool> UpdateCustomer(Customer customer)
        {
            if (customer != null)
            {
                var customerUpdate = _unitOfWork.Customers.SingleOrDefault(customer, en => en.CustomerId ==customer.CustomerId);
                if (customerUpdate != null)
                {
                    // customerUpdate = customer;
                    customerUpdate.Status = customer.Status;
                    customerUpdate.Point = customer.Point;
                    customerUpdate.CCCD = customer.CCCD;
                    customerUpdate.Phone = customer.Phone;
                    customerUpdate.Address = customer.Address;
                    customerUpdate.FullName = customer.FullName;
                    customerUpdate.UpdateDate = DateTime.Now;
                    _unitOfWork.Customers.Update(customerUpdate);
                    var result = _unitOfWork.Save();
                    if(result > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        
    }
}
