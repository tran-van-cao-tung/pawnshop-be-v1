using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.DTOs
{
    public class WareHouseDTO
    {
        public string WarehouseName { get; set; }
        public string WarehouseAddress { get; set; }
        public int Status { get; set; }

        public ICollection<ContractAssetDTO> ContractAssets { get; set; }
    }
}
