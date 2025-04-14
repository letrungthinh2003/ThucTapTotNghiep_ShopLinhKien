using System;
using System.Collections.Generic;

namespace LinhKienShop.Models
{
    public partial class MaGiamGium
    {
        public MaGiamGium()
        {
            DonHangMaGiamGia = new HashSet<DonHangMaGiamGium>();
        }

        public int MaGiamGiaId { get; set; }
        public string MaCode { get; set; } = null!;
        public int MaLoaiGiamGia { get; set; }
        public decimal GiaTriGiam { get; set; }
        public decimal? DonHangToiThieu { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayHetHan { get; set; }
        public bool? TrangThai { get; set; }
        public decimal? GiaTriGiamToiDa { get; set; }

        public virtual LoaiGiamGium MaLoaiGiamGiaNavigation { get; set; } = null!;
        public virtual ICollection<DonHangMaGiamGium> DonHangMaGiamGia { get; set; }
    }
}
