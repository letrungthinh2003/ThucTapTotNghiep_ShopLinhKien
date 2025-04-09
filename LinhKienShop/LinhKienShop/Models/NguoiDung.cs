using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinhKienShop.Models
{
    public partial class NguoiDung
    {
        public NguoiDung()
        {
            DanhGiaSanPhamMaNguoiDungNavigations = new HashSet<DanhGiaSanPham>();
            DanhGiaSanPhamMaNguoiPheDuyetNavigations = new HashSet<DanhGiaSanPham>();
            DonHangs = new HashSet<DonHang>();
            LichSuGuiEmails = new HashSet<LichSuGuiEmail>();
            TinTucs = new HashSet<TinTuc>();
            XacThucQuenMatKhaus = new HashSet<XacThucQuenMatKhau>();
            TrangThai = "HoatDong";
        }

        [Key]
        [Required(ErrorMessage = "Mã người dùng không được để trống")]
        [DisplayName("Mã người dùng")]
        public int MaNguoiDung { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [DisplayName("Email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(8, ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự")]
        [DisplayName("Mật khẩu")]
        public string MatKhau { get; set; } = null!;

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [DisplayName("Họ tên")]
        public string HoTen { get; set; } = null!;

        [StringLength(15, MinimumLength = 10, ErrorMessage = "Số điện thoại phải có 10 chữ số")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Số điện thoại không đúng định dạng")]
        [DisplayName("Số điện thoại")]
        public string? SoDienThoai { get; set; }

        [DisplayName("Địa chỉ")]
        public string? DiaChi { get; set; }

        [Required(ErrorMessage = "Vai trò không được để trống")]
        [DisplayName("Vai trò")]
        [ForeignKey("MaVaiTroNavigation")] // Liên kết với MaVaiTroNavigation
        public int MaVaiTro { get; set; }

        [DisplayName("Trạng thái")]
        public string TrangThai { get; set; } = null!;

        [DisplayName("Ngày tạo")]
        public DateTime NgayTao { get; set; }

        public virtual VaiTro? MaVaiTroNavigation { get; set; } // Đánh dấu là nullable (tùy chọn)

        public virtual ICollection<DanhGiaSanPham> DanhGiaSanPhamMaNguoiDungNavigations { get; set; }
        public virtual ICollection<DanhGiaSanPham> DanhGiaSanPhamMaNguoiPheDuyetNavigations { get; set; }
        public virtual ICollection<DonHang> DonHangs { get; set; }
        public virtual ICollection<LichSuGuiEmail> LichSuGuiEmails { get; set; }
        public virtual ICollection<TinTuc> TinTucs { get; set; }
        public virtual ICollection<XacThucQuenMatKhau> XacThucQuenMatKhaus { get; set; }
    }
}