using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using Services.Services;
using Services.Services.IServices;
using System.Collections.Generic;

namespace PawnShopBE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class PawnableProductController : ControllerBase
    {

        private readonly IPawnableProductService _pawnableProductService;
        private readonly IAttributeService _attributeService;
        private readonly IMapper _mapper;
        public PawnableProductController(IPawnableProductService pawnableProductService,
                                         IAttributeService attributeService,
                                         IMapper mapper)
        {
            _pawnableProductService = pawnableProductService;
            _attributeService = attributeService;
            _mapper = mapper;
        }

        [HttpPost("pawnable")]
        public async Task<IActionResult> CreatePawnableProduct([FromBody] PawnableProductDTO request)
        {
            PawnableProduct pawnableProduct = _mapper.Map<PawnableProduct>(request);
            var createPawnableProduct = await _pawnableProductService.CreatePawnableProduct(pawnableProduct);
            var response = true;
            if (createPawnableProduct)
            {
                List<Core.Models.Attribute> listAttribute = new List<Core.Models.Attribute>();
                foreach (Core.Models.Attribute attribute in pawnableProduct.Attributes.ToList())
                {
                    listAttribute.Add(attribute);
                }
                try
                {
                    await _attributeService.CreateAttribute(listAttribute);
                }
                catch (SqlException sql)
                {
                    return Ok(response);
                }
            }

            
          
          
           return BadRequest();
        }


        [HttpPut("pawnable{id}")]
        public async Task<IActionResult> UpdatePawnableProduct(int pawnableId, PawnableProductDTO request)
        {
                     
                var pawnableProduct = _mapper.Map<PawnableProduct>(request);
                pawnableProduct.PawnableProductId = pawnableId;

                var response = await _pawnableProductService.UpdatePawnableProduct(pawnableProduct);
                if (response)
                {
                    return Ok(response);
                }
            
            return BadRequest();
        }


    }
}
