using Microsoft.AspNetCore.Http;
using PawnShopBE.Infrastructure.Helpers;
using PawnShopBE.Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PawnShopBE.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Services.Services.IServices;
using PawnShopBE.Core.Const;

namespace PawnShopBE.Controllers
{
    [Route("api/authentication/login/")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly DbContextClass _context;
        private IAuthentication _authen;
        public AuthenticationController(DbContextClass context,IAuthentication authentication) 
        {
            _context = context;
            _authen = authentication;
        }
        [HttpPost("renewToken")]
        public async Task<IActionResult> RenewToken(TokenModel tokenmodel)
        {
            var token = tokenmodel;
            if (token != null)
            {
               var respone = await _authen.RenewToken(token);
                return Ok(respone);
            }
            return BadRequest();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(Login login)
        {
            var result=await _authen.Login(login);
            var user=_context.User.SingleOrDefault(p => p.UserName == login.userName &&
            p.Password == login.password);
            if (user != null)
            {
                return Ok(user);
            }
            var admin= _context.Admin.SingleOrDefault(p => p.UserName == login.userName &&
            p.Password== login.password);
            if(admin != null)
            {
                // cấp token
                var token = await _authen.GenerateToken(admin);
                if (token != null)
                {
                    return Ok(new
                    {
                        account=admin,
                        token= token
                    });
                }
            }
            return BadRequest(new
            {
                result = "Invalid UserName or Password"
            });
        }
        [HttpGet]
        public async Task<IActionResult> getAllToken()
        {
            var respone = await _authen.getAllToken();
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest();
        }

        
    }
}
