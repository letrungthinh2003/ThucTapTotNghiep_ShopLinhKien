using System;
using System.Collections.Generic;

namespace LinhKienShop.Models
{
    public partial class TrangThaiThanhToan
    {
        public TrangThaiThanhToan()
        {
            ThanhToans = new HashSet<ThanhToan>();
        }

        public int MaTrangThaiThanhToan { get; set; }
        public string TenTrangThai { get; set; } = null!;
        public string? MoTa { get; set; }

        public virtual ICollection<ThanhToan> ThanhToans { get; set; }
    }
}
