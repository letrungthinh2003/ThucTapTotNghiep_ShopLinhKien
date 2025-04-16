using System;
using System.ComponentModel.DataAnnotations;

namespace LinhKienShop.Models
{
    public class MaGiamGiaViewModel
    {
        public int MaGiamGiaId { get; set; }

        [Required(ErrorMessage = "Mã code là bắt buộc")]
        [StringLength(20, ErrorMessage = "Mã code không được vượt quá 20 ký tự")]
        [Display(Name = "Mã code")]
        public string MaCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn loại giảm giá")]
        [Display(Name = "Loại giảm giá")]
        public int MaLoaiGiamGia { get; set; }

        [Required(ErrorMessage = "Giá trị giảm là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá trị giảm phải >= 0")]
        [Display(Name = "Giá trị giảm")]
        public decimal GiaTriGiam { get; set; }

        [Display(Name = "Giá trị giảm tối đa")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá trị giảm tối đa phải >= 0")]
        public decimal? GiaTriGiamToiDa { get; set; }

        [Display(Name = "Đơn hàng tối thiểu")]
        [Range(0, double.MaxValue, ErrorMessage = "Đơn hàng tối thiểu phải >= 0")]
        public decimal? DonHangToiThieu { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu là bắt buộc")]
        [Display(Name = "Ngày bắt đầu")]
        [DataType(DataType.DateTime)]
        public DateTime NgayBatDau { get; set; }

        [Required(ErrorMessage = "Ngày hết hạn là bắt buộc")]
        [Display(Name = "Ngày hết hạn")]
        [DataType(DataType.DateTime)]
        public DateTime NgayHetHan { get; set; }

        [Display(Name = "Trạng thái")]
        public bool TrangThai { get; set; } = true;

        public string TrangThaiText => TrangThai ? "Hoạt động" : "Bị khóa";
    }
}