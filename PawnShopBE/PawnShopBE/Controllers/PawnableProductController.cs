﻿using AutoMapper;
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
        [HttpGet("pawnable")]
        public async Task<IActionResult> GetAllPawnable()
        {
            var respone = await _pawnableProductService.GetAllPawnableProducts();
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpPost("pawnable")]
        public async Task<IActionResult> CreatePawnable(PawnableDTO pawnable)
        {
            var pawnableMapper = _mapper.Map<PawnableProduct>(pawnable);
            var respone = await _pawnableProductService.CreatePawnableProduct(pawnableMapper);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpPut("pawnable/{id}")]
        public async Task<IActionResult> UpdatePawnableProduct(int pawnableId, PawnableDTO request)
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