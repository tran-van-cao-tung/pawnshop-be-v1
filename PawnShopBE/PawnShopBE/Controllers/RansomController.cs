using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using Services.Services;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class RansomController : ControllerBase
    {
        private readonly IRansomService _ranSomeservices;
        private readonly IMapper _mapper;

        public RansomController(IRansomService ransomService, IMapper mapper)
        {
            _ranSomeservices = ransomService;
            _mapper = mapper;
        }

        [HttpGet("ransom")]
        public async Task<IActionResult> GetAllRansom()
        {
            var respone = await _ranSomeservices.GetRansom();
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }
        //[HttpPut("ransomeBeforeEndDate/{id}")]
        //public async Task<IActionResult> RansomeBeforeEndDate(PawnableDTO request)
        //{

        //    var pawnableProduct = _mapper.Map<PawnableProduct>(request);
        //    pawnableProduct.PawnableProductId = id;

        //    var response = await _ranSomeservices.(pawnableProduct);
        //    if (response)
        //    {
        //        return Ok(response);
        //    }

        //    return BadRequest();
        //}
    }
}
