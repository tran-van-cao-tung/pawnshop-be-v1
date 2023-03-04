using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Org.BouncyCastle.Utilities;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customer;
        private readonly IContractService _contract;
        private readonly IBranchService _branch;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerService customer, IContractService contract,
           IBranchService branch, IMapper mapper) {
            _customer = customer;
            _branch= branch;
            _contract = contract;   
            _mapper = mapper;
        }

        [HttpPost("customer/{id}")]
        public async Task<IActionResult> CreateCustomer(CustomerDTO customer)
        {
            var customerMap= _mapper.Map<Customer>(customer);
            var respone=await _customer.CreateCustomer(customerMap);
            if (respone)
            {
                return Ok(respone);
            }
            return BadRequest(respone);
        }

        [HttpGet("customer")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var listCustomer = await _customer.GetAllCustomer();
            var respone = _mapper.Map<IEnumerable<DisplayCustomer>>(listCustomer);
            if (respone != null)
            {
                int i = 1;
                foreach (var customer in respone)
                {
                    // lấy customer id
                    var customerId = customer.customerId;
                    // lấy branchName
                    customer.nameBranch =await GetBranchName(customerId,listCustomer);
                    customer.numerical = i++;
                }
                return Ok(respone);
            }
            return NotFound();
        }
        private async Task<string> GetBranchName(Guid customerId,IEnumerable<Customer> listCustomer)
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
            var branchName = listContract.Join(listBranch,c=> c.BranchId,b=> b.BranchId,
                (c, b) => { return b.BranchName; });
            var name = branchName.First().ToString();
            return name;
        }
        [HttpGet("customer/{id}")]
        public async Task<IActionResult> GetCustomerById(Guid id)
        {
            var listCustomer= await _customer.GetCustomerById(id);
            if(listCustomer == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpDelete("customer/{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var listCustomer=await _customer.DeleteCustomer(id);
            if (!listCustomer)
            {
                return BadRequest();
            }
            return Ok(listCustomer);
        }

        [HttpPut("customer/{id}")]
        public async Task<IActionResult> UpdateCustomer(Guid id, Customer customer)
        {
            if (customer != null)
            {
                    var respone = await _customer.UpdateCustomer(customer);
                    if (respone)
                    {
                        return Ok(respone);
                    }
                    return BadRequest();
            }
                return BadRequest();
        }

        [HttpGet("customer/{cccd}")]
        public async Task<IActionResult> GetCustomerByCCCD(string cccd)
        {
            var customer = await _customer.getCustomerByCCCD(cccd);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
