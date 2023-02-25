using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.DTOs
{
   public class JobDTO
    {
        public Guid CustomerId { get; set; }
        public string WorkLocation { get; set; }
        public decimal Salary { get; set; }
        public bool IsWork { get; set; }
        public string LaborContract { get; set; }
    }
}
