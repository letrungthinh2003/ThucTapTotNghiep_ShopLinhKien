using System;
using System.Collections.Generic;

namespace LinhKienShop.Models
{
    public partial class TrangThaiDonHang
    {
        public TrangThaiDonHang()
        {
            DonHangs = new HashSet<DonHang>();
        }

        public int MaTrangThaiDonHang { get; set; }
        public string TenTrangThai { get; set; } = null!;
        public string? MoTa { get; set; }

        public virtual ICollection<DonHang> DonHangs { get; set; }
    }
}
