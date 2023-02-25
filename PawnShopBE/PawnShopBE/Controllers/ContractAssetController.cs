﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ContractAssetController : ControllerBase
    {
        private readonly IContractAssetService _contractAssetService;
        private readonly IMapper _mapper;

        public ContractAssetController(IContractAssetService contractAssetService, IMapper mapper) 
        { 
        _contractAssetService=contractAssetService;
            _mapper=mapper;
        }

        [HttpGet("contractAsset")]
        public async Task<IActionResult> GetAllContractAsset() {
            var respone =await _contractAssetService.GetAllContractAssets();
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpPost("contractAsset")]
        public async Task<IActionResult> CreateContractAsset(ContractAssetDTO contractAsset)
        {
            var contractAssetMapper = _mapper.Map<ContractAsset>(contractAsset);
            var respone = await _contractAssetService.CreateContractAsset(contractAssetMapper);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpDelete("contractAsset/{id}")]
        public async Task<IActionResult> DeleteContractAsset(int id)
        {
            var respone = await _contractAssetService.DeleteContractAsset(id);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpPut("contractAsset/{id}")]
        public async Task<IActionResult> UpdateContractAsset(int id,ContractAssetDTO contractAsset)
        {
            var contractAssetUpdate=_mapper.Map<ContractAsset>(contractAsset);
            contractAssetUpdate.ContractAssetId = id;
            var respone = await _contractAssetService.UpdateContractAsset(contractAssetUpdate);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }
    }
}
