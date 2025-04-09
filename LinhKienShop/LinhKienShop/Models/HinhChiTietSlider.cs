using System;
using System.Collections.Generic;

namespace LinhKienShop.Models
{
    public partial class HinhChiTietSlider
    {
        public int MaHinhChiTietSlider { get; set; }
        public int MaSanPham { get; set; }
        public string LinkHinhAnh { get; set; } = null!;
        public string LoaiHinhAnh { get; set; } = null!;

        public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
    }
}
