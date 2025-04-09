using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LinhKienShop.Models
{
    public partial class SanPham
    {
        public SanPham()
        {
            ChiTietDonHangs = new HashSet<ChiTietDonHang>();
            DanhGiaSanPhams = new HashSet<DanhGiaSanPham>();
            HinhChiTietSliders = new HashSet<HinhChiTietSlider>();
            TrangThai = "HoatDong"; // Giá trị mặc định khớp với DB
        }

        [Key]
        [DisplayName("Mã sản phẩm")]
        public int MaSanPham { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [DisplayName("Tên sản phẩm")]
        public string TenSanPham { get; set; } = null!;

        [Required(ErrorMessage = "Giá gốc là bắt buộc")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá gốc phải lớn hơn 0")]
        [DisplayName("Giá gốc (VNĐ)")]
        public decimal GiaGoc { get; set; }

        [Required(ErrorMessage = "Giá khuyến mãi là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá khuyến mãi phải lớn hơn hoặc bằng 0")]
        [DisplayName("Giá khuyến mãi (VNĐ)")]
        public decimal GiaKhuyenMai { get; set; }

        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0")]
        [DisplayName("Số lượng")]
        public int SoLuong { get; set; }

        [Required(ErrorMessage = "Danh mục là bắt buộc")]
        [DisplayName("Danh mục")]
        public int MaDanhMuc { get; set; }

        [Required(ErrorMessage = "Thương hiệu là bắt buộc")]
        [DisplayName("Thương hiệu")]
        public int MaThuongHieu { get; set; }

        [Required(ErrorMessage = "Xuất xứ là bắt buộc")]
        [DisplayName("Xuất xứ")]
        public int MaXuatXu { get; set; }

        [NotMapped]
        [DisplayName("Hình sản phẩm")]
        public IFormFile? HinhSanPhamUpload { get; set; }

        // Thuộc tính mới để upload nhiều hình ảnh chi tiết
        [NotMapped]
        [DisplayName("Hình ảnh chi tiết")]
        public List<IFormFile>? HinhChiTietUploads { get; set; } = new List<IFormFile>();

        [DisplayName("Đường dẫn hình sản phẩm")]
        [Column("HinhSanPham")]
        public string? HinhSanPhamPath { get; set; }

        [DisplayName("Thông số kỹ thuật")]
        public string? ThongSoKyThuat { get; set; }

        [DisplayName("Ngày tạo")]
        public DateTime NgayTao { get; set; } = DateTime.Now; // Giá trị mặc định trong code

        [Required(ErrorMessage = "Trạng thái là bắt buộc")]
        [DisplayName("Trạng thái")]
        public string TrangThai { get; set; }

        [ForeignKey("MaDanhMuc")]
        [DisplayName("Danh mục")]
        public virtual DanhMuc? MaDanhMucNavigation { get; set; } // Có thể null vì chỉ là navigation

        [ForeignKey("MaThuongHieu")]
        [DisplayName("Thương hiệu")]
        public virtual ThuongHieu? MaThuongHieuNavigation { get; set; } // Có thể null vì chỉ là navigation

        [ForeignKey("MaXuatXu")]
        [DisplayName("Xuất xứ")]
        public virtual XuatXu? MaXuatXuNavigation { get; set; } // Có thể null vì chỉ là navigation

        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public virtual ICollection<DanhGiaSanPham> DanhGiaSanPhams { get; set; }
        public virtual ICollection<HinhChiTietSlider> HinhChiTietSliders { get; set; }
    }
}