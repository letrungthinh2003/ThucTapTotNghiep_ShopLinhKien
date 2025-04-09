using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LinhKienShop.Models
{
    public partial class DanhMuc
    {
        public DanhMuc()
        {
            SanPhams = new HashSet<SanPham>();
        }

        [Key]
        [Required(ErrorMessage = "Mã danh mục không được để trống")]
        [DisplayName("Mã danh mục")]
        public int MaDanhMuc { get; set; }
        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [DisplayName("Tên danh mục")]
        public string TenDanhMuc { get; set; } = null!;

        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
