using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public RoleController(IRoleService roleService, IMapper mapper) 
        { 
        _roleService=roleService;
            _mapper=mapper;
        }

        [HttpGet("role")]
        public async Task<IActionResult> GetAllRole() {
            var respone =await _roleService.GetAllRole();
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpPost("role")]
        public async Task<IActionResult> CreateRole(RoleDTO role)
        {
            var roleMapper = _mapper.Map<Role>(role);
            var respone = await _roleService.CreateRole(roleMapper);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        //[HttpDelete("job/{id}")]
        //public async Task<IActionResult> DeleteJob(int id)
        //{
        //    var respone = await _jobService.DeleteJob(id);
        //    if (respone != null)
        //    {
        //        return Ok(respone);
        //    }
        //    return BadRequest();
        //}

        //[HttpPut("job/{id}")]
        //public async Task<IActionResult> UpdateJob(int id,JobDTO job)
        //{
        //    var jobUpdate=_mapper.Map<Job>(job);
        //    jobUpdate.JobId = id;
        //    var respone = await _jobService.UpdateJob(jobUpdate);
        //    if (respone != null)
        //    {
        //        return Ok(respone);
        //    }
        //    return BadRequest();
        //}
    }
}
