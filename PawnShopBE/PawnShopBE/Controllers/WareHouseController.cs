using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1")]
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

        [HttpGet("warehouse")]
        public async Task<IActionResult> GetAllWareHouse() {
            var respone =await _wareHouseService.GetWareHouse();
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpPost("warehouse")]
        public async Task<IActionResult> CreateWareHouse(WareHouseDTO wareHouse)
        {
            var wareHouseMapper = _mapper.Map<Warehouse>(wareHouse);
            var respone = await _wareHouseService.CreateWareHouse(wareHouseMapper);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpDelete("warehouse/{id}")]
        public async Task<IActionResult> DeleteWareHouse(int id)
        {
            var respone = await _wareHouseService.DeleteWareHouse(id);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpPut("warehouse/{id}")]
        public async Task<IActionResult> UpdateWareHouse(int id,WareHouseDTO wareHouse)
        {
            var wareHouseUpdate=_mapper.Map<Warehouse>(wareHouse);
            wareHouseUpdate.WarehouseId = id;
            var respone = await _wareHouseService.UpdateWareHouse(wareHouseUpdate);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }
    }
}
