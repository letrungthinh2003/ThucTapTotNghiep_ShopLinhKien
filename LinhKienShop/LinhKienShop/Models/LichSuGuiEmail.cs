using System;
using System.Collections.Generic;

namespace LinhKienShop.Models
{
    public partial class LichSuGuiEmail
    {
        public int MaLichSuGuiEmail { get; set; }
        public int MaNguoiDung { get; set; }
        public string LoaiEmail { get; set; } = null!;
        public string? TieuDe { get; set; }
        public string? NoiDung { get; set; }
        public DateTime? NgayGui { get; set; }

        public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;
    }
}
