
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
        [DisplayName("Mã loại giảm giá")]
        public int MaLoaiGiamGia { get; set; }
        [DisplayName("Tên loại giảm giá")]
        public string TenLoaiGiamGia { get; set; } = string.Empty;
        [DisplayName("Mô tả")]
        public string? MoTa { get; set; }

        public virtual ICollection<MaGiamGium> MaGiamGia { get; set; }
    }
}
