﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PawnShopBE.Core.Models
{
    public class User//:IdentityUser<Guid>
    {
        public Guid UserId { get; set; }
        public int RoleId { get; set; }
        public int? BranchId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int Status { get; set; }

        //relationship

        public virtual Role Role { get; set; }
        public virtual Branch? Branch { get; set; }
        public ICollection<Contract> Contracts { get; set; }
        public ICollection<UserPermissionGroup> UserPermissionGroups { get; set; }
        public User()
        {
            Contracts = new List<Contract>();
            UserPermissionGroups = new List<UserPermissionGroup>();
        }


    }
}
