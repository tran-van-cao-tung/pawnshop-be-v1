using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using Services.Services;
using Services.Services.IServices;
using Attribute = PawnShopBE.Core.Models.Attribute;

namespace PawnShopBE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class AttributeController : ControllerBase
    {
        private readonly IAttributeService _attributeService;
        private readonly IMapper _mapper;
        public AttributeController(IAttributeService attributeService, IMapper mapper)
        {
            _attributeService = attributeService;
            _mapper = mapper;
        }
        [HttpPost("attribute")]
        public async Task<IActionResult> CreateAttribute(AttributeDTO attribute)
        {
            var attributeMapper = _mapper.Map<Attribute>(attribute);
            var respone = await _attributeService.CreateAttribute(attributeMapper);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }
        [HttpGet("attribute/{id}")]
        public async Task<IActionResult> GetAttributesById(int pawnableId)
        {
            var attributes = await _attributeService.GetAttributeByPawnableId(pawnableId);

            if (attributes != null)
            {
                return Ok(attributes);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
