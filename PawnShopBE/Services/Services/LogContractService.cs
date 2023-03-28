using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class LogContractService : ILogContractService
    {
        public IUnitOfWork _unitOfWork;

        public LogContractService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CreateLogContract(LogContract logContract)
        {
            if (logContract != null)
            {
                logContract.LogTime = DateTime.Now;
                await _unitOfWork.LogContracts.Add(logContract);

                var result = _unitOfWork.Save();
                return (result > 0) ? true : false;
            }
            return false;
        }

        public Task<IEnumerable<LogContract>> GetLogContracts(int num)
        {
            throw new NotImplementedException();
        }
    }
}
