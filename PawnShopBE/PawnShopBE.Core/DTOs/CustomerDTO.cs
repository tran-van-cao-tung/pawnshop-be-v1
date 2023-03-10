using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.DTOs
{
    public class CustomerDTO
    {
        public int? KycId { get; set; }

        [Required(ErrorMessage = "Tên khách hàng không được để trống")]
        [StringLength(50, MinimumLength = 6,
          ErrorMessage = "Tên khách hàng phải dài từ 6 - 50 ký tự")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Căn cước công dân không được để trống")]
        public string CCCD { get; set; }

        [Required(ErrorMessage = "Địa chỉ khách hàng không được để trống")]
        public string Address { get; set; }

        [Phone(ErrorMessage = "Nhập đúng định dạng số điện thoại")]
        public string Phone { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int Status { get; set; }
        public int Point { get; set; }
        public ICollection<DependentPeopleDTO>? DependentPeople { get; set; }
        public ICollection<JobDTO>? Jobs { get; set; }
        public ICollection<CustomerRelativeDTO>? CustomerRelativeRelationships { get; set; }
    }
}
