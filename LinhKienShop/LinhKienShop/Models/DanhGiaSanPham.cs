using System;
using System.Collections.Generic;

namespace LinhKienShop.Models
{
    public partial class DanhGiaSanPham
    {
        public int MaDanhGia { get; set; }
        public int MaSanPham { get; set; }
        public int MaNguoiDung { get; set; }
        public int SoSaoDanhGia { get; set; }
        public string? BinhLuan { get; set; }
        public DateTime? NgayDanhGia { get; set; }
        public bool? TrangThaiPheDuyet { get; set; }
        public int? MaNguoiPheDuyet { get; set; }

        public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;
        public virtual NguoiDung? MaNguoiPheDuyetNavigation { get; set; }
        public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
    }
}
