using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.DTOs
{
    public class LedgerDTO
    {
        public int LedgerId { get; set; }
        public decimal ReceivedPrincipal { get; set; }
        public decimal RecveivedInterest { get; set; }
        public decimal Loan { get; set; }
        public long Balance { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Status { get; set; }
    }
}
