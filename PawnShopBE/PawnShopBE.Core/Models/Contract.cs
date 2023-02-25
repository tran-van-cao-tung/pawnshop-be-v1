using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace PawnShopBE.Core.Models
{
    public class Contract
    {
        public int ContractId { get; set; }
        public Guid CustomerId { get; set; }
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


        public virtual Customer Customer { get; set; }
        public virtual Package Package { get; set; }
        public virtual Branch Branch { get; set; }
        public virtual ContractAsset ContractAsset { get; set; }
        public virtual Liquidtation Liquidtation { get; set; }
        public ICollection<InterestDiary> InterestDiaries { get; set; }
        public Contract()
        {
            InterestDiaries = new List<InterestDiary>();
        }



    }
}
