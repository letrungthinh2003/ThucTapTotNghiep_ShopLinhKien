using System;
using System.Collections.Generic;

namespace LinhKienShop.Models
{
    public partial class ThongTinCuaHang
    {
        public int MaThongTin { get; set; }
        public string? TenCuaHang { get; set; }
        public string? DuongDanLogo { get; set; }
        public string? Email { get; set; }
        public string? SoDienThoai { get; set; }
        public string? DiaChi { get; set; }
        public string? LienKetFacebook { get; set; }
        public string? LienKetYoutube { get; set; }
        public string? LienKetInstagram { get; set; }
        public string? BanQuyen { get; set; }
        public DateTime? NgayCapNhat { get; set; }
    }
}
