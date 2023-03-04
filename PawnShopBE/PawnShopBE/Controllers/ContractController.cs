﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using PawnShopBE.Core.Const;
using PawnShopBE.Core.Requests;
using Services.Services;
using Services.Services.IServices;
using System.Text;

namespace PawnShopBE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IContractService _contractService;
        private readonly ICustomerService _customerService;
        private readonly IContractAssetService _contractAssetService;
        private readonly IPackageService _packageService;
        private readonly IInteresDiaryService _interesDiaryService;

        private readonly IMapper _mapper;

        public ContractController(
            IContractService contractService, 
            ICustomerService customer, 
            IContractAssetService contractAssetService, 
            IPackageService packageService,
            IInteresDiaryService interestDiaryService,
            IMapper mapper)
        {
            _contractService = contractService;
            _customerService = customer;
            _contractAssetService = contractAssetService;
            _packageService = packageService;
            _interesDiaryService = interestDiaryService;
            _mapper = mapper;
        }

        [HttpPost("contract")]
        public async Task<IActionResult> CreateContract(ContractDTO request)
        {
            StringBuilder sb = new StringBuilder();
            foreach (AttributeDTO attributes in request.PawnableAttributeDTOs)
            {            
                sb.Append(attributes.Description + "/");              
            }       
            //Get package
            Package package = await _packageService.GetPackageById(request.PackageId, request.InterestRecommend);
            //Get customer
            var customer = await _customerService.getCustomerByCCCD(request.cccd); 
            
            //Create asset
            var contractAsset = _mapper.Map<ContractAsset>(request);
                contractAsset.Description = sb.ToString();
            await _contractAssetService.CreateContractAsset(contractAsset);
            
            // Create contract
            var contract = _mapper.Map<Contract>(request);
            contract.ContractAssetId = contractAsset.ContractAssetId;
            contract.CustomerId = customer.CustomerId;
            var contractSuccess = await _contractService.CreateContract(contract);
            var interestDiarySuccess = await _interesDiaryService.CreateInteresDiary(contract);
            
            if (interestDiarySuccess == true && interestDiarySuccess == true)
            {
                return Ok(interestDiarySuccess); 
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("contracts")]
        public async Task<IActionResult> GetAllContracts()
        {
            var listContracts = await _contractService.GetAllContracts();
            if (listContracts == null)
            {
                return NotFound();
            }
            return Ok(listContracts);
        }

        [HttpPut("contract")]
        public async Task<IActionResult> UpdateContract(ContractDTO request)
        {       
                var contract = _mapper.Map<Contract>(request);
                var response = await _contractService.UpdateContract(contract);
                if (response)
                {
                    return Ok(response);
                }         
            return BadRequest();
        }
    }
}
