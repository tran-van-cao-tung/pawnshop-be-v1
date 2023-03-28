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
    public class LogContractRepository : GenericRepository<LogContract>, ILogContractRepository
    {
        public LogContractRepository(DbContextClass dbContext) : base(dbContext)
        {

        }
    }
}
