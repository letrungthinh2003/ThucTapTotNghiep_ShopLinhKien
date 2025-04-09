using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace LinhKienShop.Models
{
    public partial class VaiTro
    {
        public VaiTro()
        {
            NguoiDungs = new HashSet<NguoiDung>();
        }

        [Key]
        [Required(ErrorMessage = "Mã vai trò không được để trống")]
        [DisplayName("Mã vai trò")]
        public int MaVaiTro { get; set; }
        [DisplayName("Tên vai trò")]
        public string TenVaiTro { get; set; } = null!;

        public virtual ICollection<NguoiDung> NguoiDungs { get; set; }
    }
}
