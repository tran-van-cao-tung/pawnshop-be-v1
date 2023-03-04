using PawnShopBE.Core.Const;
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
    public class CustomerService : ICustomerService
    {
        public IUnitOfWork _unitOfWork;
        public IKycService _kycService;
        public ICustomerRepository _customerRepository;
        public CustomerService(IUnitOfWork unitOfWork, IKycService kycService, ICustomerRepository customerRepository) {
            _unitOfWork= unitOfWork;
            _kycService= kycService;
            _customerRepository= customerRepository;
        }
        public async Task<bool> CreateCustomer(Customer customer)
        {
            var oldCus = GetCustomerById(customer.CustomerId);
            // Check if new customer
            if (oldCus == null)
            {       
                customer.Kyc = null;
                customer.DependentPeople = null;
                customer.Jobs = null;
                customer.CustomerRelativeRelationships = null;
                customer.Status = (int)CustomerConst.ACTIVE;
                customer.Point = 0;
                customer.CreatedDate = DateTime.Now;
                await _unitOfWork.Customers.Add(customer);
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

        public async Task<Customer> getCustomerByCCCD(string cccd)
        {
            if (cccd != null)
            {
                var customer = await _customerRepository.getCustomerByCCCD(cccd);
                if (customer != null)
                {
                    return customer;
                }
            }
            return null;
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
