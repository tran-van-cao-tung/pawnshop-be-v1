using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Infrastructure.Repositories
{
    public class ContractRepository : GenericRepository<Contract>, IContractRepository
    {
        private readonly DbContextClass _dbContextClass;
        public ContractRepository(DbContextClass dbContext) : base(dbContext)
        {
            _dbContextClass = dbContext;
        }

        public async Task<Contract> getContractByContractCode(string contractCode)
        {
            Contract contract = _dbContext.Contract.SingleOrDefault(c => c.ContractCode == contractCode);
            return contract;
        }
    }
}
