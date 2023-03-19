using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Org.BouncyCastle.Utilities;
using PawnShopBE.Core.Display;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using PawnShopBE.Core.Validation;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customer;
       
        private readonly IMapper _mapper;

        public CustomerController(ICustomerService customer, IMapper mapper) {
            _customer = customer;
            _mapper = mapper;
        }
        [HttpGet("getRelative/{id}")]
        public async Task<IActionResult> getCustomerRelative(Guid id)
        {
            var respone= await _customer.getRelative(id);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpPost("createRelative/{id}")]
        public async Task<IActionResult> createCustomerRelative(Guid id, CustomerDTO customer)
        {
            var respone = await _customer.createRelative(id,customer);
            if (respone)
            {
                return Ok();
            }
            return BadRequest();
        }
        private Validation<CustomerDTO> _validation=new Validation<CustomerDTO>();
      
        [HttpPost("createCustomer")]
        public async Task<IActionResult> CreateCustomer(CustomerDTO customer)
        {
          //  Check Validation
            var checkValidation = await _validation.CheckValidation(customer);
            if (checkValidation != null)
            {
                return BadRequest(checkValidation);
            }
            var customerMap= _mapper.Map<Customer>(customer);
            var respone=await _customer.CreateCustomer(customerMap);
            if (respone)
            {
                return Ok(respone);
            }
            return BadRequest(respone);
        }

        [HttpGet("getAll/{numPage}")]
        public async Task<IActionResult> GetAllCustomers(int numPage)
        {
            var listCustomer = await _customer.GetAllCustomer(numPage);
            var respone = _mapper.Map<IEnumerable<DisplayCustomer>>(listCustomer);
            if (respone != null)
            {
                respone = await _customer.getCustomerHaveBranch(respone, listCustomer);
                return Ok(respone);
            }
            return NotFound();
        }
       
        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetCustomerById(Guid id)
        {
            var customer= await _customer.GetCustomerById(id);
            if(customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpDelete("deleteCustomer/{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var listCustomer=await _customer.DeleteCustomer(id);
            if (!listCustomer)
            {
                return BadRequest();
            }
            return Ok(listCustomer);
        }

        [HttpPut("updateCustomer/{id}")]
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

        [HttpGet("getByCCCD/{cccd}")]
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
