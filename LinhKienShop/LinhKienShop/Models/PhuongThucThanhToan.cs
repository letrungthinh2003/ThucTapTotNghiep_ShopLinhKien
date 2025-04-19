using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace LinhKienShop.Models
{
    public partial class PhuongThucThanhToan
    {
        public PhuongThucThanhToan()
        {
            DonHangs = new HashSet<DonHang>();
        }

        [Key]
        [Required(ErrorMessage = "Mã phương thức thanh toán không được để trống")]
        [DisplayName("Mã phương thức thanh toán")]
        public int MaPhuongThucThanhToan { get; set; }
        [DisplayName("Tên phương thức thanh toán")]
        public string TenPhuongThucThanhToan { get; set; } = null!;
        [DisplayName("Mô tả")]
        public string? MoTa { get; set; }

        public virtual ICollection<DonHang> DonHangs { get; set; }
    }
}
