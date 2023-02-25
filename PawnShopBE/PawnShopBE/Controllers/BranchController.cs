using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;
        private readonly IMapper _mapper;
        public BranchController(IBranchService branchService, IMapper mapper)
        {
            _branchService = branchService;
            _mapper = mapper;
        }

        [HttpPost("branch")]
        public async Task<IActionResult> CreateBranch(BranchDTO request)
        {
            var branch = _mapper.Map<Branch>(request);
            var response = await _branchService.CreateBranch(branch);

            if (response)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("branches")]
        public async Task<IActionResult> GetBranchList()
        {
            var branchList = await _branchService.GetAllBranch();
            if (branchList == null)
            {
                return NotFound();
            }
            return Ok(branchList);
        }

        [HttpGet("branch/{id}")]
        public async Task<IActionResult> GetBranchById(int branchId)
        {
            var branch = await _branchService.GetBranchById(branchId);

            if (branch != null)
            {
                return Ok(branch);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("branch/{id}")]
        public async Task<IActionResult> UpdateBranch(int id, BranchDTO request)
        {
            if (id == request.BranchId)
            {
                var branch = _mapper.Map<Branch>(request);
                var response = await _branchService.UpdateBranch(branch);
                if (response)
                {
                    return Ok(response);
                }
            }
            return BadRequest();
        }

        [HttpDelete("branch/{id}")]
        public async Task<IActionResult> DeleteBranch(int branchId)
        {
            var isBranchCreated = await _branchService.DeleteBranch(branchId);

            if (isBranchCreated)
            {
                return Ok(isBranchCreated);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
