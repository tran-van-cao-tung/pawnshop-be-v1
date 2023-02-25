using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.Const;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using Services.Services.IServices;

namespace PawnShopBE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService,IRoleService roleService, IMapper mapper)
        {
            _userService = userService;
            _roleService = roleService;
            _mapper = mapper;
        }

        [HttpPost("user")]
        public async Task<IActionResult> CreateUser(UserDTO request)
        {  
            var user = _mapper.Map<User>(request);
            var response = await _userService.CreateUser(user);

            if (response)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("users")]
        public async Task<IActionResult> getUserList()
        {
            var userList = await _userService.GetAllUsers();
            if (userList == null)
            {
                return NotFound();
            }
            return Ok(userList);
        }

        [HttpGet("user/{id:guid}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var user = await _userService.GetUserById(userId);

            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("user/{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid id, UserDTO request)
        {
          
            if (request != null && request.UserId == id)
            {
                var user = _mapper.Map<User>(request);
                var response = await _userService.UpdateUser(user);
                if (response)
                {
                    return Ok(response);
                }
                return BadRequest();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("user/{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var isUserCreated = await _userService.DeleteUser(userId);

            if (isUserCreated)
            {
                return Ok(isUserCreated);
            }
            else
            {
                return BadRequest();
            }
        }
    
    }
}
