using System;
using System.Collections.Generic;

namespace LinhKienShop.Models
{
    public partial class DonHangMaGiamGium
    {
        public int DonHangMaGiamGiaId { get; set; }
        public int MaDonHang { get; set; }
        public int MaGiamGiaId { get; set; }
        public decimal SoTienGiam { get; set; }
        public DateTime? NgayApDung { get; set; }

        public virtual DonHang MaDonHangNavigation { get; set; } = null!;
        public virtual MaGiamGium MaGiamGia { get; set; } = null!;
    }
}
