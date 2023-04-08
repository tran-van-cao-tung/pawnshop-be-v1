﻿using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.Data;
using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
    public interface IAuthentication
    {
        Task<TokenModel> GenerateToken(User? user,Admin? admin);
        //Task<ApiRespone> RenewToken(TokenModel tokenModel);
        Task<IEnumerable<RefeshToken>> getAllToken();
        Task<bool> Login(Login user);
        ClaimsPrincipal EncrypToken(string token);
    }
}
