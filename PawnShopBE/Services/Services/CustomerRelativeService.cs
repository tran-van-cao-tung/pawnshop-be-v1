using PawnShopBE.Core.Display;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using Services.Services.IServices;
using Services.Services.IThirdInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class CustomerRelativeService : ICustomer_Relative
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CustomerRelativeRelationship customerRelativeRelationship;
        public CustomerRelativeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> CreateCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreateCustomerRelative(CustomerRelativeRelationship customerRelative)
        {
            if (customerRelative != null)
            {
                await _unitOfWork.CustomersRelativeRelationships.Add(customerRelative);
                var result = _unitOfWork.Save();
                if (await plusPoint(customerRelative))
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
        private async Task<bool> plusPoint(CustomerRelativeRelationship relatives)
        {
            var customerList = await GetAllCustomer(0);
            var customerIenumerable = from c in customerList where c.CustomerId == relatives.CustomerId select c;
            var customer = new Customer();
            customer = customerIenumerable.FirstOrDefault();
            //plus point
            customer.Point += 25;
            if (await UpdateCustomer(customer))
                return true;
            else
                return false;
        }

        public Task<bool> createRelative(Guid idCus, CustomerDTO customer)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCustomer(Guid customerId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteCustomerRelative(Guid CustomerRelativeId)
        {
            if (CustomerRelativeId != null)
            {
                var customer = _unitOfWork.CustomersRelativeRelationships.
                    SingleOrDefault(customerRelativeRelationship, j => j.CustomerRelativeRelationshipId == CustomerRelativeId);
                if (customer != null)
                {
                    _unitOfWork.CustomersRelativeRelationships.Delete(customer);
                    var result = _unitOfWork.Save();
                    if (result > 0)
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

        public async Task<IEnumerable<Customer>> GetAllCustomer(int num)
        {
            var listCustomer = await _unitOfWork.Customers.GetAll();
            if (num == 0)
            {
                return listCustomer;
            }
            var result = await _unitOfWork.Customers.TakePage(num, listCustomer);
            return result;
        }

        public Task<Customer> getCustomerByCCCD(string cccd)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> GetCustomerById(Guid idCus)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DisplayCustomer>> getCustomerHaveBranch(IEnumerable<DisplayCustomer> respone, IEnumerable<Customer> listCustomer)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CustomerRelativeRelationship>> GetCustomerRelative()
        {
            var listCustomer = await _unitOfWork.CustomersRelativeRelationships.GetAll();
             return listCustomer;
        }

        public Task<Job> GetCustomerRelativeById(Guid CustomerRelativeId)
        {
            throw new NotImplementedException();
        }

        public Task<CustomerDTO> getRelative(Guid idCus)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateCustomer(Customer customer)
        {
            if (customer != null)
            {
                var customerUpdate = _unitOfWork.Customers.SingleOrDefault(customer, en => en.CustomerId == customer.CustomerId);
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
                    if (result > 0)
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

        public async Task<bool> UpdateCustomerRelative(CustomerRelativeRelationship customerRelative)
        {
            if (customerRelative != null)
            {
                var customerUpdate = _unitOfWork.CustomersRelativeRelationships
                    .SingleOrDefault(customerRelative, en => en.CustomerRelativeRelationshipId
                    == customerRelative.CustomerRelativeRelationshipId);
                if (customerUpdate != null)
                {
                    // customerUpdate = customer;
                    customerUpdate.RelativeRelationship = customerRelative.RelativeRelationship;
                    customerUpdate.Salary = customerRelative.Salary;
                    customerUpdate.Address= customerRelative.Address;
                    customerUpdate.RelativePhone = customerRelative.RelativePhone;
                    customerUpdate.CustomerId = customerRelative.CustomerId;
                    customerUpdate.RelativeName = customerRelative.RelativeName;
                    _unitOfWork.CustomersRelativeRelationships.Update(customerUpdate);
                    var result = _unitOfWork.Save();
                    if (result > 0)
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
