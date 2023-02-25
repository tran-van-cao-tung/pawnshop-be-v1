using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.DTOs
{
    public class ContractAssetDTO
    {
        public int WarehouseId { get; set; }
        public int PawnableProductId { get; set; }
        public string SerialCode { get; set; }
        public string ContractAssetName { get; set; }
        public string? Description { get; set; }
        public string Image { get; set; }
        public int Status { get; set; }
    }
}
