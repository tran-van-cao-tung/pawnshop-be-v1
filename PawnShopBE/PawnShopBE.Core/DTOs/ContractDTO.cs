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
        public int? ContractId { get; set; }
        public Guid? CustomerId { get; set; }
        public int PackageId { get; set; }
        public int BranchId { get; set; }
        public int ContractAssetId { get; set; }
        public string ContractCode { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractEndDate { get; set; }
        public DateTime ActualEndDate { get; set; }
        public decimal Loan { get; set; }
        public decimal? InsuranceFee { get; set; }
        public decimal? StorageFee { get; set; }
        public decimal CustomerRecieved { get; set; }
        public string Description { get; set; }
        public string ContractVerifying { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int Status { get; set; }

        // Customer DTO
        
        public int KycId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Point { get; set; }

        // ContractAsset
        // AttributeAsset
        // PawnAbleAsset
        // PackageAsset

    }
}
