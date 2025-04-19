using Microsoft.EntityFrameworkCore;
using LinhKienShop.Models;

namespace LinhKienShop.Models
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }


        public  DbSet<Banner> Banners { get; set; } = null!;
        public  DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; } = null!;
        public  DbSet<DanhGiaSanPham> DanhGiaSanPhams { get; set; } = null!;
        public  DbSet<DanhMuc> DanhMucs { get; set; } = null!;
        public  DbSet<DiaChiGiaoHang> DiaChiGiaoHangs { get; set; } = null!;
        public  DbSet<DonHang> DonHangs { get; set; } = null!;
        public  DbSet<HinhChiTietSlider> HinhChiTietSliders { get; set; } = null!;
        public  DbSet<LichSuGuiEmail> LichSuGuiEmails { get; set; } = null!;
        public  DbSet<LoaiGiamGium> LoaiGiamGia { get; set; } = null!;
        public  DbSet<MaGiamGium> MaGiamGia { get; set; } = null!;
        public  DbSet<NguoiDung> NguoiDungs { get; set; } = null!;
        public  DbSet<PhuongThucThanhToan> PhuongThucThanhToans { get; set; } = null!;
        public  DbSet<SanPham> SanPhams { get; set; } = null!;
        public  DbSet<ThongTinCuaHang> ThongTinCuaHangs { get; set; } = null!;
        public  DbSet<ThuongHieu> ThuongHieus { get; set; } = null!;
        public  DbSet<TinTuc> TinTucs { get; set; } = null!;
        public  DbSet<VaiTro> VaiTros { get; set; } = null!;
        public  DbSet<XacThucQuenMatKhau> XacThucQuenMatKhaus { get; set; } = null!;
        public  DbSet<XuatXu> XuatXus { get; set; } = null!;
    }
}
