using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.DTOs
{
   public class CustomerRelativeDTO
    {
        public Guid CustomerId { get; set; }
        public string RelativeRelationship { get; set; }
        public string RelativeName { get; set; }
        public decimal? Salary { get; set; }
        public string Address { get; set; }
        public bool AddressVerify { get; set; }
        public string RelativePhone { get; set; }
        public bool RelativePhoneVerify { get; set; }
    }
}
