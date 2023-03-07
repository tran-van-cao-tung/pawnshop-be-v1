using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.Display;
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
        [Authorize]
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
        [HttpGet("branch/detail/{id}")]
        public async Task<IActionResult> GetBranchDetail(int id)
        {
            var branchList = await _branchService.GetBranchById(id);
            var branchDetail = _mapper.Map<DisplayBranchDetail>(branchList);
            if (branchDetail != null)
            {
                branchDetail = await _branchService.getDisplayBranchDetail(branchDetail);
                return Ok(branchDetail);
            }
            return BadRequest();
        }
        [HttpGet("branch/chain")]
        public async Task<IActionResult> GetBranchChain()
        {
            var branchList = await _branchService.GetAllBranch();
            var displayBranch = _mapper.Map<IEnumerable<DisplayBranch>>(branchList);
            if (displayBranch != null)
            {
                displayBranch = await _branchService.getDisplayBranch(displayBranch);
                return Ok(displayBranch);
            }
            return NotFound();
        }
        [HttpGet("branch")]
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
        public async Task<IActionResult> GetBranchById(int id)
        {
            var branch = await _branchService.GetBranchById(id);

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
        public async Task<IActionResult> UpdateBranch(int id, Branch request)
        {
            if (id == request.BranchId)
            {
               // var branch = _mapper.Map<Branch>(request);
                var response = await _branchService.UpdateBranch(request);
                if (response)
                {
                    return Ok(response);
                }
            }
            return BadRequest();
        }
        [Authorize]
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
