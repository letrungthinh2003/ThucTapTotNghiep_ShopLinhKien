using System;
using System.Collections.Generic;

namespace LinhKienShop.Models
{
    public partial class DiaChiGiaoHang
    {
        public int MaDiaChiGiaoHang { get; set; }
        public int MaNguoiDung { get; set; }
        public string HoTenNguoiNhan { get; set; } = null!;
        public string SoDienThoai { get; set; } = null!;
        public string DiaChiChiTiet { get; set; } = null!;
        public string? ThanhPho { get; set; }
        public string? QuanHuyen { get; set; }
        public string? PhuongXa { get; set; }
        public bool LaMacDinh { get; set; } = false; // Sửa từ bool? thành bool, mặc định là false
        public DateTime? NgayTao { get; set; }

        public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;
    }
}