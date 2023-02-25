using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
    public interface IWareHouseService
    {
        Task<bool> CreateWareHouse(Warehouse warehouse);
        Task<IEnumerable<Warehouse>> GetWareHouse();
        Task<Warehouse> GetWareHouseById(int wareHouseId);
        Task<bool> UpdateWareHouse(Warehouse warehouse);
        Task<bool> DeleteWareHouse(int wareHouseId);
    }
}
