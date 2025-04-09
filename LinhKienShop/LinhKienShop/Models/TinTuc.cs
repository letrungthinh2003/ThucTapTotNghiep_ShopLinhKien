using System;
using System.Collections.Generic;

namespace LinhKienShop.Models
{
    public partial class TinTuc
    {
        public int MaTinTuc { get; set; }
        public string TieuDe { get; set; } = null!;
        public string NoiDung { get; set; } = null!;
        public string? HinhAnhDaiDien { get; set; }
        public int? MaNguoiTao { get; set; }
        public DateTime NgayTao { get; set; }
        public bool? TrangThai { get; set; }

        public virtual NguoiDung? MaNguoiTaoNavigation { get; set; }
    }
}
