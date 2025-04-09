using System;
using System.Collections.Generic;

namespace LinhKienShop.Models
{
    public partial class ThanhToan
    {
        public int MaThanhToan { get; set; }
        public int MaDonHang { get; set; }
        public decimal SoTien { get; set; }
        public DateTime NgayThanhToan { get; set; }
        public int MaPhuongThucThanhToan { get; set; }
        public int MaTrangThaiThanhToan { get; set; }
        public string? GhiChu { get; set; }

        public virtual DonHang MaDonHangNavigation { get; set; } = null!;
        public virtual PhuongThucThanhToan MaPhuongThucThanhToanNavigation { get; set; } = null!;
        public virtual TrangThaiThanhToan MaTrangThaiThanhToanNavigation { get; set; } = null!;
    }
}
