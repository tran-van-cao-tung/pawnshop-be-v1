﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Models
{
    public class ContractAsset
    {
        public int ContractAssetId { get; set; }
        public int WarehouseId { get; set; }
        public int PawnableProductId { get; set; }
        public string SerialCode { get; set; } 
        public string ContractAssetName { get; set; }
        public string? Description { get; set; }
        public string Image { get; set; }
        public int Status { get; set; }



        public virtual Warehouse Warehouse { get; set; }
        public virtual PawnableProduct PawnableProduct { get; set; }
        public ICollection<Contract> Contracts { get; set; }
        public ContractAsset()
        {
            Contracts = new List<Contract>();
        }
        
    }
}
