using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
    public interface IContractService
    {
        Task<bool> CreateContract(Contract contract);

        Task<IEnumerable<Contract>> GetAllContracts();

        Task<Contract> GetContractById(int contractId);

        Task<bool> UpdateContract(Contract contract);

        Task<bool> DeleteContract(int contractId);
    }
}
