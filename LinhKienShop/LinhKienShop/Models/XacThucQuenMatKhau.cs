using System;
using System.Collections.Generic;

namespace LinhKienShop.Models
{
    public partial class XacThucQuenMatKhau
    {
        public int MaXacThucQuenMatKhau { get; set; }
        public int MaNguoiDung { get; set; }
        public string Token { get; set; } = null!;
        public DateTime ThoiGianHetHan { get; set; }
        public bool? DaSuDung { get; set; }

        public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;
    }
}
