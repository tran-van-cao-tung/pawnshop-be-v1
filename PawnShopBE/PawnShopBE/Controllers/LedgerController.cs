using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using PawnShopBE.Core.Validation;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class LedgerController : ControllerBase
    {
        private readonly ILedgerService _ledgerService;
        private readonly IMapper _mapper;

        public LedgerController(ILedgerService ledgerService, IMapper mapper) 
        { 
        _ledgerService=ledgerService;
            _mapper=mapper;
        }

        [HttpGet("ledger")]
        public async Task<IActionResult> GetAllLedger() {
            var respone =await _ledgerService.GetLedger();
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }
        private Validation<LedgerDTO> _validation;
        
    [HttpPost("ledger")]
        public async Task<IActionResult> CreateLedger(LedgerDTO ledger)
        {
            //Check Validation
            var checkValidation = await _validation.CheckValidation(ledger);
            if (checkValidation != null)
            {
                return BadRequest(checkValidation);
            }
            var ledgerMapper = _mapper.Map<Ledger>(ledger);
            var respone = await _ledgerService.CreateLedger(ledgerMapper);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpDelete("ledger/{id}")]
        public async Task<IActionResult> DeleteLedger(int id)
        {
            var respone = await _ledgerService.DeleteLedger(id);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpPut("ledger/{id}")]
        public async Task<IActionResult> UpdateLedger(int id,LedgerDTO ledger)
        {
            var ledgerUpdate=_mapper.Map<Ledger>(ledger);
            ledgerUpdate.LedgerId = id;
            var respone = await _ledgerService.UpdateLedger(ledgerUpdate);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }
    }
}
