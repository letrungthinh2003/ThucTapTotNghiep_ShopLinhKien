using System;
using System.Collections.Generic;

namespace LinhKienShop.Models
{
    public partial class PhuongThucThanhToan
    {
        public PhuongThucThanhToan()
        {
            ThanhToans = new HashSet<ThanhToan>();
        }

        public int MaPhuongThucThanhToan { get; set; }
        public string TenPhuongThucThanhToan { get; set; } = null!;
        public string? MoTa { get; set; }

        public virtual ICollection<ThanhToan> ThanhToans { get; set; }
    }
}
