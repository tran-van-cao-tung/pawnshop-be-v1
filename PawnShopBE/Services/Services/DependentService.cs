using PawnShopBE.Core.Display;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using Services.Services.IServices;
using Services.Services.IThirdInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class DependentService : ICustomer_Dependent
    {
        private readonly IUnitOfWork _unit;
        public readonly DependentPeople dependentPeople;
        public DependentService(IUnitOfWork unitOfWork)
        {
            _unit = unitOfWork;
        }
        public async Task<bool> CreateDependent(DependentPeople dependent)
        {
            if (dependent != null)
            {
                await _unit.DependentPeople.Add(dependent);
                var result = _unit.Save();
                if (await plusPoint(dependent))
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> DeleteDependent(Guid dependentId)
        {
            var dependentDelete = _unit.DependentPeople.SingleOrDefault(dependentPeople, j => j.DependentPeopleId == dependentId);
            if (dependentDelete != null)
            {
                _unit.DependentPeople.Delete(dependentDelete);
                var result = _unit.Save();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<IEnumerable<DependentPeople>> GetDependent()
        {
            var result = await _unit.DependentPeople.GetAll();
            return result;
        }

        public Task<DependentPeople> GetDependentById(Guid dependentId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateDependent(DependentPeople dependent)
        {
            var dependentUpdate = _unit.DependentPeople.SingleOrDefault(dependent, j => j.DependentPeopleId == dependent.DependentPeopleId);
            if (dependentUpdate != null)
            {
                dependentUpdate.DependentPeopleName = dependent.DependentPeopleName;
                dependentUpdate.CustomerRelationShip = dependent.CustomerRelationShip;
                dependentUpdate.MoneyProvided = dependent.MoneyProvided;
                dependentUpdate.Address = dependent.Address;
                dependentUpdate.PhoneNumber= dependent.Address;
                dependentUpdate.CustomerId = dependent.CustomerId;
                _unit.DependentPeople.Update(dependentUpdate);
                var result = _unit.Save();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }
        private async Task<bool> plusPoint(DependentPeople dependents)
        {
            var customerList = await GetAllCustomer(0);
            var customerIenumerable = from c in customerList where c.CustomerId == dependents.CustomerId select c;
            var customer = new Customer();
            customer = customerIenumerable.FirstOrDefault();
            //plus point
            customer.Point += 25;
            if (await UpdateCustomer(customer))
                return true;
            else
                return false;
        }

        public Task<bool> CreateCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Customer>> GetAllCustomer(int num)
        {
            var listCustomer = await _unit.Customers.GetAll();
            if (num == 0)
            {
                return listCustomer;
            }
            var result = await _unit.Customers.TakePage(num, listCustomer);
            return result;
        }

        public Task<Customer> GetCustomerById(Guid idCus)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateCustomer(Customer customer)
        {
            if (customer != null)
            {
                var customerUpdate = _unit.Customers.SingleOrDefault(customer, en => en.CustomerId == customer.CustomerId);
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
                    _unit.Customers.Update(customerUpdate);
                    var result = _unit.Save();
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

        public Task<bool> DeleteCustomer(Guid customerId)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> getCustomerByCCCD(string cccd)
        {
            throw new NotImplementedException();
        }

        public Task<CustomerDTO> getRelative(Guid idCus)
        {
            throw new NotImplementedException();
        }

        public Task<bool> createRelative(Guid idCus, CustomerDTO customer)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DisplayCustomer>> getCustomerHaveBranch(IEnumerable<DisplayCustomer> respone, IEnumerable<Customer> listCustomer)
        {
            throw new NotImplementedException();
        }
    }
}
