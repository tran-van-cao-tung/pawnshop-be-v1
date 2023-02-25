using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMapper _mapper;

        public CustomerController(ICustomerService customer, IMapper mapper) {
            _customer = customer;
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
            if (listCustomer == null)
            {
                return NotFound();
            }
            return Ok(listCustomer);
        }

        [HttpGet("customer/{id}")]
        public async Task<IActionResult> GetCustomerById(Guid id)
        {
            var listCustomer= await _customer.GetCustomerById(id);
            if(listCustomer == null)
            {
                return NotFound();
            }
            return Ok(listCustomer);
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
        public async Task<IActionResult> UpdateCustomer(Guid id, CustomerDTO customer)
        {
            if (customer != null)
            {
                var customerMapper = _mapper.Map<Customer>(customer);
                customerMapper.CustomerId= id;
                    var respone = await _customer.UpdateCustomer(customerMapper);
                    if (respone)
                    {
                        return Ok(respone);
                    }
                    return BadRequest();
            }
                return BadRequest();
        }

    }
}
