using System;
using System.Collections.Generic;

namespace LinhKienShop.Models
{
    public partial class DonHang
    {
        public DonHang()
        {
            ChiTietDonHangs = new HashSet<ChiTietDonHang>();
        }

        public int MaDonHang { get; set; }
        public int MaNguoiDung { get; set; }
        public int MaDiaChiGiaoHang { get; set; }
        public DateTime NgayDatHang { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public decimal TongTien { get; set; }
        public decimal GiamGia { get; set; }
        public int MaGiamGiaId { get; set; }
        public int MaPhuongThucThanhToan { get; set; }
        public decimal PhiShip { get; set; }
        public string TrangThai { get; set; } = null!;
        public bool DaThanhToan { get; set; }
        public string? GhiChu { get; set; }

        public virtual DiaChiGiaoHang MaDiaChiGiaoHangNavigation { get; set; } = null!;
        public virtual MaGiamGium MaGiamGia { get; set; } = null!;
        public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;
        public virtual PhuongThucThanhToan MaPhuongThucThanhToanNavigation { get; set; } = null!;
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
    }
}
