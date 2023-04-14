using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.Models;
using PawnShopBE.Core.Requests;
using Services.Services;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1/logAsset")]
    [ApiController]
    [Authorize]
    public class LogAssetController : ControllerBase
    {
        private readonly ILogAssetService _logAssetService;
        public LogAssetController(ILogAssetService logAssetService)
        {
            _logAssetService = logAssetService;
        }

        [HttpPut("updateLogAsset")]
        public async Task<IActionResult> UpdateLogAsset(LogAsset logAsset)
        {
            if (logAsset != null)
            {
                var response = await _logAssetService.UpdateLogAsset(logAsset);
                if (response)
                {
                    return Ok(response);
                }
            }
            return BadRequest();
        }

        [HttpGet("getLogAssetsByAssetId/{assetId}")]
        public async Task<IActionResult> GetLogAssetByAssetId(int assetId)
        {
            var respone = await _logAssetService.LogAssetByAssetId(assetId);
            if (respone != null)
            {
                return Ok(respone);
            }
            return NotFound(respone);
        }
    }
}
