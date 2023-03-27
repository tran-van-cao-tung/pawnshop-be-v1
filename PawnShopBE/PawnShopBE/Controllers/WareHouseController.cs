﻿using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.Data.DescriptionAttribute;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Core.Validation;
using Services.Services.IServices;
using System.ComponentModel.DataAnnotations;

namespace PawnShopBE.Controllers
{
    [Route("api/v1/warehouse")]
    [ApiController]
    public class WareHouseController : ControllerBase
    {
        private readonly IWareHouseService _wareHouseService;
        private readonly IMapper _mapper;
       

        public WareHouseController(IWareHouseService wareHouseService, IMapper mapper) 
        { 
        _wareHouseService= wareHouseService;
            _mapper=mapper;
        }

        [HttpGet("GetAllDetail/{id},{numPage}")]
        public async Task<IActionResult> GetAllWareHouseDetail(int id,int numPage)
        {
            var respone = await _wareHouseService.getWareHouseDetail(id,numPage);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }
        [HttpGet("GetAll/{numPage}")]
        public async Task<IActionResult> GetAllWareHouse(int numPage) {
            
                var respone = await _wareHouseService.GetWareHouse(numPage);

                if (respone != null)
                {
                    return Ok(respone);
                }
                else
                {
                    return BadRequest();
                }
        }
         private Validation<WareHouseDTO> _validation=new Validation<WareHouseDTO>();
        [HttpPost("createWarehouse")]
        public async Task<IActionResult> CreateWareHouse(WareHouseDTO wareHouse)
        {
            //Check Validation
            var checkValidation =await _validation.CheckValidation(wareHouse);
            if (checkValidation != null)
            {
                return BadRequest(checkValidation);
            }

            var wareHouseMapper = _mapper.Map<Warehouse>(wareHouse);
            var respone = await _wareHouseService.CreateWareHouse(wareHouseMapper);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpDelete("deleteWarehouse/{id}")]
        public async Task<IActionResult> DeleteWareHouse(int id)
        {
            var respone = await _wareHouseService.DeleteWareHouse(id);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpPut("updateWareHouse")]
        public async Task<IActionResult> UpdateWareHouse(WareHouseDTO wareHouse)
        {
            var wareHouseUpdate=_mapper.Map<Warehouse>(wareHouse);
            var respone = await _wareHouseService.UpdateWareHouse(wareHouseUpdate);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }
    }
}
