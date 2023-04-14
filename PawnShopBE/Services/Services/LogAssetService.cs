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
    public class LogAssetService : ILogAssetService

    {
        private readonly IUnitOfWork _unit;
        private readonly ILogAssetRepository _logAssetRepository;
        public LogAssetService(IUnitOfWork unit, ILogAssetRepository logAssetRepository)
        {
            _unit = unit;
            _logAssetRepository = logAssetRepository;
        }
        public async Task<bool> CreateLogAsset(LogAsset logAsset)
        {
            await _unit.LogAssets.Add(logAsset);
            var result = _unit.Save();
            if (result > 0)
                return true;

            return false;
        }

        public async Task<IEnumerable<LogAsset>> LogAssetByAssetId(int contractAssetId)
        {
            var logAssetList = await _logAssetRepository.GetAllByAssetId(contractAssetId);
            return (logAssetList != null) ? logAssetList : null;
        }

        public async Task<bool> UpdateLogAsset(LogAsset logAsset)
        {
            var logAssetUpdate =  _unit.LogAssets.SingleOrDefault(logAsset, l => l.logAssetId == logAsset.logAssetId);
            if (logAssetUpdate != null)
            {
                logAssetUpdate.ImportImg = logAsset.ImportImg;
                logAssetUpdate.ExportImg = logAsset.ExportImg;
                logAssetUpdate.Description = logAsset.Description;
                _unit.LogAssets.Update(logAssetUpdate);
                var result = _unit.Save();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }
        
    }
}
