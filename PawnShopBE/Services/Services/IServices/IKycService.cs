﻿using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
    public interface IKycService
    {
        Task<bool> CreateKyc(Kyc kyc);
        Task<bool> DeleteKyc(int idKyc);
        Task<bool> UpdateKyc(Kyc kyc);
        Task<IEnumerable<Kyc>> GetAllKyc();
    }
}
