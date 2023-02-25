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
    public class WareHouseService : IWareHouseService
    {
        private readonly IUnitOfWork _unit;
        private readonly Warehouse warehouse;

        public WareHouseService(IUnitOfWork unitOfWork) { 
              _unit=unitOfWork;
        }
        public async Task<bool> CreateWareHouse(Warehouse warehouse)
        {
            if ( warehouse!= null)
            {
                await _unit.Warehouses.Add(warehouse);
                var result = _unit.Save();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> DeleteWareHouse(int wareHouseId)
        {

           var wareHouseDelete=  _unit.Warehouses.
                SingleOrDefault(warehouse,j=> j.WarehouseId == wareHouseId);
            if (wareHouseDelete != null)
            {
                _unit.Warehouses.Delete(wareHouseDelete);
                var result = _unit.Save();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<IEnumerable<Warehouse>> GetWareHouse()
        {
            var result = await _unit.Warehouses.GetAll();
            return result;
        }

        public Task<Warehouse> GetWareHouseById(int wareHouseId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateWareHouse(Warehouse warehouse)
        {
            var wareHouseUpdate = _unit.Warehouses.SingleOrDefault
                (warehouse, j => j.WarehouseId == warehouse.WarehouseId);
            if(wareHouseUpdate!= null)
            {
                wareHouseUpdate.WarehouseName=warehouse.WarehouseName;
                wareHouseUpdate.WarehouseAddress=warehouse.WarehouseAddress;
                wareHouseUpdate.Status=warehouse.Status;
                _unit.Warehouses.Update(wareHouseUpdate);
                var result= _unit.Save();
                if(result> 0)
                {
                    return true;
                }
            }
            return false;
        }
        
    }
}
