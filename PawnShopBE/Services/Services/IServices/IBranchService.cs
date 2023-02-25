using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
    public interface IBranchService
    {
        Task<bool> CreateBranch(Branch branch);

        Task<IEnumerable<Branch>> GetAllBranch();

        Task<Branch> GetBranchById(int branchId);

        Task<bool> UpdateBranch(Branch branch);

        Task<bool> DeleteBranch(int branchId);  
    }
}
