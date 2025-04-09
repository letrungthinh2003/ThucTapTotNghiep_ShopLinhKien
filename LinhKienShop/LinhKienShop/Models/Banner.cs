using System;
using System.Collections.Generic;

namespace LinhKienShop.Models
{
    public partial class Banner
    {
        public int MaBanner { get; set; }
        public string TenBanner { get; set; } = null!;
        public string DuongDanAnh { get; set; } = null!;
        public string? DuongDanLienKet { get; set; }
        public string? GhiChu { get; set; }
        public DateTime NgayTao { get; set; }
    }
}
