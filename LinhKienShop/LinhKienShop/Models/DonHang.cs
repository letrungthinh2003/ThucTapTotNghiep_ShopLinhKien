using System;
using System.Collections.Generic;

namespace LinhKienShop.Models
{
    public partial class DonHang
    {
        public DonHang()
        {
            ChiTietDonHangs = new HashSet<ChiTietDonHang>();
            DonHangMaGiamGia = new HashSet<DonHangMaGiamGium>();
            ThanhToans = new HashSet<ThanhToan>();
        }

        public int MaDonHang { get; set; }
        public string MaDonHangText { get; set; } = null!;
        public int MaNguoiDung { get; set; }
        public DateTime NgayDatHang { get; set; }
        public decimal TongTien { get; set; }
        public decimal PhiVanChuyen { get; set; }
        public decimal TongTienSauGiam { get; set; }
        public int MaTrangThaiDonHang { get; set; }
        public string? GhiChu { get; set; }

        public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;
        public virtual TrangThaiDonHang MaTrangThaiDonHangNavigation { get; set; } = null!;
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public virtual ICollection<DonHangMaGiamGium> DonHangMaGiamGia { get; set; }
        public virtual ICollection<ThanhToan> ThanhToans { get; set; }
    }
}
