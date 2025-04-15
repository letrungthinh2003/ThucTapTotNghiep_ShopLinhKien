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

        [Required(ErrorMessage = "Tên loại giảm giá là bắt buộc.")]
        [StringLength(20, ErrorMessage = "Tên loại giảm giá không được vượt quá 20 ký tự.")]
        public string TenLoaiGiamGia { get; set; } = string.Empty;
        [DisplayName("Mô tả")]
        public string? MoTa { get; set; }

        public virtual ICollection<MaGiamGium> MaGiamGia { get; set; }
    }
}