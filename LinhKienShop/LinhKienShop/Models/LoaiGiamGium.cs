using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace LinhKienShop.Models
{
    public partial class LoaiGiamGium
    {
        public LoaiGiamGium()
        {
            MaGiamGia = new HashSet<MaGiamGium>();
        }
        [Key]
        [Required(ErrorMessage = "Mã loại giảm giá không được để trống")]
        [DisplayName("Mã loại giảm giá")]
        public int MaLoaiGiamGia { get; set; }
        [DisplayName("Tên loại giảm giá")]
        public string TenLoaiGiamGia { get; set; } = null!;
        [DisplayName("Mô tả")]
        public string? MoTa { get; set; }

        public virtual ICollection<MaGiamGium> MaGiamGia { get; set; }
    }
}
