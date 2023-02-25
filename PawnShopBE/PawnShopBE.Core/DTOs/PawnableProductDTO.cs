using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attribute = PawnShopBE.Core.Models.Attribute;

namespace PawnShopBE.Core.DTOs
{
    public class PawnableProductDTO
    {
        public int PawnableProductId { get; }
        public string TypeOfProduct { get; set; }
        public string CommodityCode { get; set; }
        public int Status { get; set; }

        public ICollection<AttributeDTO>? AttributeDTOs { get; set; }
    }
}
