using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace LinhKienShop.Models
{
    public partial class ThuongHieu
    {
        public ThuongHieu()
        {
            SanPhams = new HashSet<SanPham>();
        }

        [Key]
        [Required(ErrorMessage = "Mã thương hiệu không được để trống")]
        [DisplayName("Mã thương hiệu")]
        public int MaThuongHieu { get; set; }
        [DisplayName("Tên thương hiệu")]
        public string TenThuongHieu { get; set; } = null!;

        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}