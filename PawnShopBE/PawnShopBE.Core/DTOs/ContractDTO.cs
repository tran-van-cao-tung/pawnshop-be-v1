using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.DTOs
{
    public class ContractDTO
    {
        public Guid CustomerId { get; }
        public Guid UserId { get; set; }
        public int BranchId { get; set; }
        public int WarehouseId { get; set; }
        public int PawnableProductId { get; set; }
        public int ContractAssetId { get; }
        public string CustomerName { get; set; }
        public string IdentityCard { get; set; }
        public string ContractCode { get; set; }
        public string ContractAssetName { get; set; }
        public decimal InsuranceFee { get; set; }
        public decimal StorageFee { get; set; }
        public decimal Loan { get; set; }
        public decimal CustomerRecived { get; set; }
        public string AssetImg { get; set; }
        public ICollection<AttributeDTO>? PawnableAttributeDTOs { get; set; }

        public ContractDTO()
        {
            InsuranceFee = 0;
            StorageFee = 0;
        }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerAddress { get; set; }
        public int PackageId { get; set; }
        public int InterestRecommend { get; set; }
        public string? Description { get; set; }
    }
}
