using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class CustomerRelativeController : ControllerBase
    {
        private readonly ICustomerRelativeService _customerRelative;
        private readonly IMapper _mapper;

        public CustomerRelativeController(ICustomerRelativeService customerRelativeService, IMapper mapper) 
        { 
        _customerRelative=customerRelativeService;
            _mapper=mapper;
        }

        [HttpGet("customerRelative")]
        public async Task<IActionResult> GetAllCustomerRelative() {
            var respone =await _customerRelative.GetCustomerRelative();
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpPost("customerRelative")]
        public async Task<IActionResult> CreateCustomerRelative(CustomerRelativeDTO customerRelative)
        {
            var customerRelativeMapper = _mapper.Map<CustomerRelativeRelationship>(customerRelative);
            var respone = await _customerRelative.CreateCustomerRelative(customerRelativeMapper);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpDelete("customerRelative/{id}")]
        public async Task<IActionResult> DeleteCustomerRelative(Guid id)
        {
            var respone = await _customerRelative.DeleteCustomerRelative(id);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpPut("customerRelative/{id}")]
        public async Task<IActionResult> UpdateCustomerRelative(Guid id,CustomerRelativeDTO customerRelative)
        {
            var customerRelativeMapper = _mapper.Map<CustomerRelativeRelationship>(customerRelative);
            customerRelativeMapper.CustomerRelativeRelationshipId = id;
            var respone = await _customerRelative.UpdateCustomerRelative(customerRelativeMapper);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }
    }
}
