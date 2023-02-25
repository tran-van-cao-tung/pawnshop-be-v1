﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly IMapper _mapper;

        public JobController(IJobService jobService, IMapper mapper) 
        { 
        _jobService=jobService;
            _mapper=mapper;
        }

        [HttpGet("job")]
        public async Task<IActionResult> GetAllJob() {
            var respone =await _jobService.GetJob();
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpPost("job")]
        public async Task<IActionResult> CreateJob(JobDTO job)
        {
            var jobMapper = _mapper.Map<Job>(job);
            var respone = await _jobService.CreateJob(jobMapper);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpDelete("job/{id}")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var respone = await _jobService.DeleteJob(id);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        [HttpPut("job/{id}")]
        public async Task<IActionResult> UpdateJob(int id,JobDTO job)
        {
            var jobUpdate=_mapper.Map<Job>(job);
            jobUpdate.JobId = id;
            var respone = await _jobService.UpdateJob(jobUpdate);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }
    }
}
