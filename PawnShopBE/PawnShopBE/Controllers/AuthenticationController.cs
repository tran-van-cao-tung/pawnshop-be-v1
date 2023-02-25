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


namespace PawnShopBE.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly DbContextClass _context;
        private readonly Appsetting _appsetting;

        public AuthenticationController(DbContextClass context,IOptionsMonitor<Appsetting> optionsMonitor) 
        {
            _context = context;
            _appsetting = optionsMonitor.CurrentValue;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Validate(Login login)
        {
            var user= _context.User.SingleOrDefault(p => p.UserName == login.userName &&
            p.Password== login.password);
            if(user == null)
            {
                return Ok(Response(false,"Invalid UserName or Password",null));
            }
            // cấp token
             var token = await Generate(user);
                return BadRequest(Response(true, "Authentication Success", token));
        }

        [HttpGet]
        private  async Task<TokenModel> Generate(User user)
        {
            var jwtToken = new JwtSecurityTokenHandler();
            var secretKeyByte = Encoding.UTF8.GetBytes(_appsetting.SecretKey);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim("UserName",user.UserName),
                    new Claim("Id", user.UserId.ToString()),
                }),

                Expires= DateTime.UtcNow.AddMinutes(1),
                SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(secretKeyByte),
                SecurityAlgorithms.HmacSha512Signature)
            };
            //create Token
            var token= jwtToken.CreateToken(tokenDescription);
            var accessToken =jwtToken.WriteToken(token);
            var resfulToken = GenerateRefeshToken();

            //save data
            var refeshTokenEntity = new RefeshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.UserId,
                JwtID = token.Id,
                Token = resfulToken,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddHours(1),
            };

            await _context.AddAsync(refeshTokenEntity);
            await _context.SaveChangesAsync();

            return new TokenModel
            {
                AccessToken = accessToken,
                RefeshToken = resfulToken
            };
        }
        [HttpGet]
        private string GenerateRefeshToken()
        {
            var random = new Byte[32];
            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                string token=Convert.ToBase64String(random);
                return token;
            }
        }

        [HttpGet] //Renew AccessToken
        public async Task<IActionResult> RenewToken(TokenModel tokenModel)
        {
            var jwtToken = new JwtSecurityTokenHandler();
            var secretKeyByte= Encoding.UTF8.GetBytes(_appsetting.SecretKey);
            var tokenValidateParam = new TokenValidationParameters
            {
                // tự cấp token
                ValidateIssuer = false,
                ValidateAudience = false,

                //ký vào token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyByte),
                ClockSkew = TimeSpan.Zero,

                //ko check Token hết hạn
                ValidateLifetime = false
            };

            try
            {
                //check 1: Accesstoken valid format
                var tokenInVerification = jwtToken.ValidateToken(tokenModel.AccessToken,
                    tokenValidateParam, out var validatedToken);

                //check 2: check thuat toan
                if(validatedToken is JwtSecurityToken jwtSecurity)
                {
                    var result = jwtSecurity.Header.Alg.Equals(SecurityAlgorithms.HmacSha512,
                        StringComparison.InvariantCultureIgnoreCase);
                    if(!result)
                    {
                        return BadRequest(Response(false, "InvalidToken", null));
                    }
                }

                //check 3 check time expire
                var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(
                    x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                 var expireDate= ConverUrnixTimeToDateTime(utcExpireDate);
                if(expireDate> DateTime.UtcNow)
                {
                    return BadRequest(Response(false, "Access token has not yet expired", null));
                }


                // check 4: refeshToken exist in db
                var storedToken = _context.RefeshTokens.FirstOrDefault(x => x.Token
                == tokenModel.RefeshToken);
                if(storedToken == null)
                {
                    return BadRequest(Response(false, "Refesh does not exits", null));
                }

                //check 5: refeshToken is used/revoked
                if(storedToken.IsUsed)
                {
                    return Ok(Response(false, "RefeshToken has Used", null));
                }
                if (storedToken.IsRevoked)
                {
                    return Ok(Response(false, "RefeshToken has Revoked", null));
                }

                //check 6: Access Token Id == jwtId in RefeshToken
                var jti = tokenInVerification.Claims.FirstOrDefault(x=> x.Type
                == JwtRegisteredClaimNames.Jti).Value;
                if(storedToken.JwtID != jti)
                {
                    return BadRequest(Response(false, "Token doesn't match", null));
                }

                //Update token is used
                storedToken.IsRevoked= true;
                storedToken.IsUsed = true;
                _context.Update(storedToken);
                await _context.SaveChangesAsync();

                //create new Token
                var user = await _context.User.SingleOrDefaultAsync(us => us.UserId ==
                storedToken.UserId);
                var token = await Generate(user);

                return Ok(Response(true,"Renew Token Access",null));
            }
            catch
            {
                return Ok(Response(false, "Something Went Wrong", null));
            }
        }
        [HttpGet]
        private DateTime ConverUrnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1980, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();
            return dateTimeInterval;
        }

        [HttpGet] //Respon Authentication, Status
        private ApiRespone Response(bool success, string message,object? data) {
            return new ApiRespone
            {
                Success = success,
                Message = message,
                Data = data
            };
        }
    }
}
