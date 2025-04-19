using System;
using System.Collections.Generic;

namespace LinhKienShop.Models
{
    public partial class LoaiGiamGium
    {
        public LoaiGiamGium()
        {
            MaGiamGia = new HashSet<MaGiamGium>();
        }

        public int MaLoaiGiamGia { get; set; }
        public string TenLoaiGiamGia { get; set; } = null!;
        public string? MoTa { get; set; }

        public virtual ICollection<MaGiamGium> MaGiamGia { get; set; }
    }
}
