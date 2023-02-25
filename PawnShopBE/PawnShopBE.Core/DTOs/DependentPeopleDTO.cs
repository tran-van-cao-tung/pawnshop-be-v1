﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.DTOs
{
    public class DependentPeopleDTO
    {
        public Guid CustomerId { get; set; }
        public string DependentPeopleName { get; set; }
        public string CustomerRelationShip { get; set; }
        public decimal? MoneyProvided { get; set; }
        public string Address { get; set; }
        public bool AddressVerify { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneVerify { get; set; }
    }
}
