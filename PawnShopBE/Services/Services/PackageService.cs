﻿using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class PackageService : IPackageService
    {
        public IUnitOfWork _unitOfWork;

        public PackageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Package>> GetAllPackages()
        {
            var packageList = await _unitOfWork.Packages.GetAll();
            return packageList;
        }

        public async Task<Package> GetPackageById(int packageId)
        {
            if (packageId != null)
            {
                var package = await _unitOfWork.Packages.GetById(packageId);
                if (package != null)
                {
                    return package;
                }
            }
            return null;
        }
    }
}
