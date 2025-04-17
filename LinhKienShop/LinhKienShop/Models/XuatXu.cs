
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace LinhKienShop.Models
{
    public partial class XuatXu
    {
        public XuatXu()
        {
            SanPhams = new HashSet<SanPham>();
        }

        [Key]
        [Required(ErrorMessage = "Mã xuất xứ không được để trống")]
        [DisplayName("Mã xuất xứ")]
        public int MaXuatXu { get; set; }
        [DisplayName("Tên xuất xứ")]
        public string TenXuatXu { get; set; } = null!;

        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
