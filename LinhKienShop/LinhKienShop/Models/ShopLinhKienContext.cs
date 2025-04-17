using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LinhKienShop.Models
{
    public partial class ShopLinhKienContext : DbContext
    {
        public ShopLinhKienContext()
        {
        }

        public ShopLinhKienContext(DbContextOptions<ShopLinhKienContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Banner> Banners { get; set; } = null!;
        public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; } = null!;
        public virtual DbSet<DanhGiaSanPham> DanhGiaSanPhams { get; set; } = null!;
        public virtual DbSet<DanhMuc> DanhMucs { get; set; } = null!;
        public virtual DbSet<DiaChiGiaoHang> DiaChiGiaoHangs { get; set; } = null!;
        public virtual DbSet<DonHang> DonHangs { get; set; } = null!;
        public virtual DbSet<DonHangMaGiamGium> DonHangMaGiamGia { get; set; } = null!;
        public virtual DbSet<HinhChiTietSlider> HinhChiTietSliders { get; set; } = null!;
        public virtual DbSet<LichSuGuiEmail> LichSuGuiEmails { get; set; } = null!;
        public virtual DbSet<LoaiGiamGium> LoaiGiamGia { get; set; } = null!;
        public virtual DbSet<MaGiamGium> MaGiamGia { get; set; } = null!;
        public virtual DbSet<NguoiDung> NguoiDungs { get; set; } = null!;
        public virtual DbSet<PhuongThucThanhToan> PhuongThucThanhToans { get; set; } = null!;
        public virtual DbSet<SanPham> SanPhams { get; set; } = null!;
        public virtual DbSet<ThanhToan> ThanhToans { get; set; } = null!;
        public virtual DbSet<ThongTinCuaHang> ThongTinCuaHangs { get; set; } = null!;
        public virtual DbSet<ThuongHieu> ThuongHieus { get; set; } = null!;
        public virtual DbSet<TinTuc> TinTucs { get; set; } = null!;
        public virtual DbSet<TrangThaiDonHang> TrangThaiDonHangs { get; set; } = null!;
        public virtual DbSet<TrangThaiThanhToan> TrangThaiThanhToans { get; set; } = null!;
        public virtual DbSet<VaiTro> VaiTros { get; set; } = null!;
        public virtual DbSet<XacThucQuenMatKhau> XacThucQuenMatKhaus { get; set; } = null!;
        public virtual DbSet<XuatXu> XuatXus { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=MSI-THINH;Database=ShopLinhKien;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Banner>(entity =>
            {
                entity.HasKey(e => e.MaBanner)
                    .HasName("PK__Banner__508B4A4907D31F08");

                entity.ToTable("Banner");

                entity.HasIndex(e => e.TenBanner, "UQ__Banner__B0DCE39EFA958845")
                    .IsUnique();

                entity.Property(e => e.DuongDanAnh).HasMaxLength(255);

                entity.Property(e => e.DuongDanLienKet).HasMaxLength(255);

                entity.Property(e => e.GhiChu).HasMaxLength(200);

                entity.Property(e => e.NgayTao)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TenBanner).HasMaxLength(100);
            });

            modelBuilder.Entity<ChiTietDonHang>(entity =>
            {
                entity.HasKey(e => e.MaChiTietDonHang)
                    .HasName("PK__ChiTietD__4B0B45DDD1801660");

                entity.ToTable("ChiTietDonHang");

                entity.HasIndex(e => e.MaDonHang, "IX_ChiTietDonHang_MaDonHang");

                entity.HasIndex(e => e.MaSanPham, "IX_ChiTietDonHang_MaSanPham");

                entity.Property(e => e.DonGia).HasColumnType("decimal(15, 2)");

                entity.HasOne(d => d.MaDonHangNavigation)
                    .WithMany(p => p.ChiTietDonHangs)
                    .HasForeignKey(d => d.MaDonHang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ChiTietDo__MaDon__76969D2E");

                entity.HasOne(d => d.MaSanPhamNavigation)
                    .WithMany(p => p.ChiTietDonHangs)
                    .HasForeignKey(d => d.MaSanPham)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ChiTietDo__MaSan__778AC167");
            });

            modelBuilder.Entity<DanhGiaSanPham>(entity =>
            {
                entity.HasKey(e => e.MaDanhGia)
                    .HasName("PK__DanhGiaS__AA9515BF1A4D97C6");

                entity.ToTable("DanhGiaSanPham");

                entity.HasIndex(e => e.MaNguoiDung, "IX_DanhGiaSanPham_MaNguoiDung");

                entity.HasIndex(e => e.MaSanPham, "IX_DanhGiaSanPham_MaSanPham");

                entity.Property(e => e.NgayDanhGia)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TrangThaiPheDuyet).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.MaNguoiDungNavigation)
                    .WithMany(p => p.DanhGiaSanPhamMaNguoiDungNavigations)
                    .HasForeignKey(d => d.MaNguoiDung)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DanhGiaSanPham_NguoiDung");

                entity.HasOne(d => d.MaNguoiPheDuyetNavigation)
                    .WithMany(p => p.DanhGiaSanPhamMaNguoiPheDuyetNavigations)
                    .HasForeignKey(d => d.MaNguoiPheDuyet)
                    .HasConstraintName("FK_DanhGiaSanPham_NguoiPheDuyet");

                entity.HasOne(d => d.MaSanPhamNavigation)
                    .WithMany(p => p.DanhGiaSanPhams)
                    .HasForeignKey(d => d.MaSanPham)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DanhGiaSanPham_SanPham");
            });

            modelBuilder.Entity<DanhMuc>(entity =>
            {
                entity.HasKey(e => e.MaDanhMuc)
                    .HasName("PK__DanhMuc__B3750887DBF4BB87");

                entity.ToTable("DanhMuc");

                entity.Property(e => e.TenDanhMuc).HasMaxLength(100);
            });

            modelBuilder.Entity<DiaChiGiaoHang>(entity =>
            {
                entity.HasKey(e => e.MaDiaChiGiaoHang)
                    .HasName("PK__DiaChiGi__2F156EA832105AD2");

                entity.ToTable("DiaChiGiaoHang");

                entity.Property(e => e.DiaChiChiTiet).HasMaxLength(255);

                entity.Property(e => e.HoTenNguoiNhan).HasMaxLength(100);

                entity.Property(e => e.LaMacDinh).HasDefaultValueSql("((0))");

                entity.Property(e => e.NgayTao)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PhuongXa).HasMaxLength(100);

                entity.Property(e => e.QuanHuyen).HasMaxLength(100);

                entity.Property(e => e.SoDienThoai).HasMaxLength(20);

                entity.Property(e => e.ThanhPho).HasMaxLength(100);

                entity.HasOne(d => d.MaNguoiDungNavigation)
                    .WithMany(p => p.DiaChiGiaoHangs)
                    .HasForeignKey(d => d.MaNguoiDung)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DiaChiGia__MaNgu__2B0A656D");
            });

            modelBuilder.Entity<DonHang>(entity =>
            {
                entity.HasKey(e => e.MaDonHang)
                    .HasName("PK__DonHang__129584ADE35DD2B4");

                entity.ToTable("DonHang");

                entity.HasIndex(e => e.MaNguoiDung, "IX_DonHang_MaNguoiDung");

                entity.HasIndex(e => e.MaTrangThaiDonHang, "IX_DonHang_MaTrangThaiDonHang");

                entity.HasIndex(e => e.MaDonHangText, "UQ__DonHang__B5D177806852D989")
                    .IsUnique();

                entity.Property(e => e.GhiChu).HasMaxLength(200);

                entity.Property(e => e.MaDonHangText).HasMaxLength(10);

                entity.Property(e => e.MaTrangThaiDonHang).HasDefaultValueSql("((1))");

                entity.Property(e => e.NgayDatHang)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PhiVanChuyen).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.TongTien).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.TongTienSauGiam).HasColumnType("decimal(15, 2)");

                entity.HasOne(d => d.MaNguoiDungNavigation)
                    .WithMany(p => p.DonHangs)
                    .HasForeignKey(d => d.MaNguoiDung)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DonHang__MaNguoi__6C190EBB");

                entity.HasOne(d => d.MaTrangThaiDonHangNavigation)
                    .WithMany(p => p.DonHangs)
                    .HasForeignKey(d => d.MaTrangThaiDonHang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DonHang__MaTrang__6D0D32F4");
            });

            modelBuilder.Entity<DonHangMaGiamGium>(entity =>
            {
                entity.HasKey(e => e.DonHangMaGiamGiaId)
                    .HasName("PK__DonHang___05BF2B55AAFEDB91");

                entity.ToTable("DonHang_MaGiamGia");

                entity.HasIndex(e => new { e.MaDonHang, e.MaGiamGiaId }, "UQ_DonHang_MaGiamGia")
                    .IsUnique();

                entity.Property(e => e.DonHangMaGiamGiaId).HasColumnName("DonHangMaGiamGiaID");

                entity.Property(e => e.MaGiamGiaId).HasColumnName("MaGiamGiaID");

                entity.Property(e => e.NgayApDung)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SoTienGiam).HasColumnType("decimal(15, 2)");

                entity.HasOne(d => d.MaDonHangNavigation)
                    .WithMany(p => p.DonHangMaGiamGia)
                    .HasForeignKey(d => d.MaDonHang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DonHangMaGiamGia_DonHang");

                entity.HasOne(d => d.MaGiamGia)
                    .WithMany(p => p.DonHangMaGiamGia)
                    .HasForeignKey(d => d.MaGiamGiaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DonHangMaGiamGia_MaGiamGia");
            });

            modelBuilder.Entity<HinhChiTietSlider>(entity =>
            {
                entity.HasKey(e => e.MaHinhChiTietSlider)
                    .HasName("PK__HinhChiT__CA2B77820AE2FBFE");

                entity.ToTable("HinhChiTietSlider");

                entity.HasIndex(e => e.MaSanPham, "IX_HinhChiTietSlider_MaSanPham");

                entity.Property(e => e.LinkHinhAnh)
                    .HasMaxLength(255)
                    .HasColumnName("Link_HinhAnh");

                entity.Property(e => e.LoaiHinhAnh).HasMaxLength(20);

                entity.HasOne(d => d.MaSanPhamNavigation)
                    .WithMany(p => p.HinhChiTietSliders)
                    .HasForeignKey(d => d.MaSanPham)
                    .HasConstraintName("FK__HinhChiTi__MaSan__48CFD27E");
            });

            modelBuilder.Entity<LichSuGuiEmail>(entity =>
            {
                entity.HasKey(e => e.MaLichSuGuiEmail)
                    .HasName("PK__LichSuGu__8D47E8BC9A127E46");

                entity.ToTable("LichSuGuiEmail");

                entity.Property(e => e.LoaiEmail).HasMaxLength(50);

                entity.Property(e => e.NgayGui)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TieuDe).HasMaxLength(200);

                entity.HasOne(d => d.MaNguoiDungNavigation)
                    .WithMany(p => p.LichSuGuiEmails)
                    .HasForeignKey(d => d.MaNguoiDung)
                    .HasConstraintName("FK__LichSuGui__MaNgu__1BC821DD");
            });

            modelBuilder.Entity<LoaiGiamGium>(entity =>
            {
                entity.HasKey(e => e.MaLoaiGiamGia)
                    .HasName("PK__LoaiGiam__7BA2A35D03FE11C9");

                entity.HasIndex(e => e.TenLoaiGiamGia, "UQ__LoaiGiam__9C445B80DA8C1B55")
                    .IsUnique();

                entity.Property(e => e.MoTa).HasMaxLength(255);

                entity.Property(e => e.TenLoaiGiamGia).HasMaxLength(20);
            });

            modelBuilder.Entity<MaGiamGium>(entity =>
            {
                entity.HasKey(e => e.MaGiamGiaId)
                    .HasName("PK__MaGiamGi__C28471D3E2059066");

                entity.HasIndex(e => e.MaCode, "UQ__MaGiamGi__152C7C5C6127C9DE")
                    .IsUnique();

                entity.Property(e => e.MaGiamGiaId).HasColumnName("MaGiamGiaID");

                entity.Property(e => e.DonHangToiThieu).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.GiaTriGiam).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.GiaTriGiamToiDa).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.MaCode).HasMaxLength(20);

                entity.Property(e => e.NgayBatDau).HasColumnType("datetime");

                entity.Property(e => e.NgayHetHan).HasColumnType("datetime");

                entity.Property(e => e.TrangThai).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.MaLoaiGiamGiaNavigation)
                    .WithMany(p => p.MaGiamGia)
                    .HasForeignKey(d => d.MaLoaiGiamGia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MaGiamGia__MaLoa__6477ECF3");
            });

            modelBuilder.Entity<NguoiDung>(entity =>
            {
                entity.HasKey(e => e.MaNguoiDung)
                    .HasName("PK__NguoiDun__C539D762920DEC66");

                entity.ToTable("NguoiDung");

                entity.HasIndex(e => e.MaVaiTro, "IX_NguoiDung_MaVaiTro");

                entity.HasIndex(e => e.Email, "UQ__NguoiDun__A9D1053463D41E92")
                    .IsUnique();

                entity.Property(e => e.DiaChi).HasMaxLength(200);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.HoTen).HasMaxLength(100);

                entity.Property(e => e.MatKhau).HasMaxLength(255);

                entity.Property(e => e.NgayTao)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SoDienThoai).HasMaxLength(15);

                entity.Property(e => e.TrangThai)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('HoatDong')");

                entity.HasOne(d => d.MaVaiTroNavigation)
                    .WithMany(p => p.NguoiDungs)
                    .HasForeignKey(d => d.MaVaiTro)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__NguoiDung__MaVai__52593CB8");
            });

            modelBuilder.Entity<PhuongThucThanhToan>(entity =>
            {
                entity.HasKey(e => e.MaPhuongThucThanhToan)
                    .HasName("PK__PhuongTh__D0270899F972B4EB");

                entity.ToTable("PhuongThucThanhToan");

                entity.HasIndex(e => e.TenPhuongThucThanhToan, "UQ__PhuongTh__344EF6D14C64E4EA")
                    .IsUnique();

                entity.Property(e => e.MoTa).HasMaxLength(255);

                entity.Property(e => e.TenPhuongThucThanhToan).HasMaxLength(20);
            });

            modelBuilder.Entity<SanPham>(entity =>
            {
                entity.HasKey(e => e.MaSanPham)
                    .HasName("PK__SanPham__FAC7442DE83C20C7");

                entity.ToTable("SanPham");

                entity.HasIndex(e => e.MaDanhMuc, "IX_SanPham_MaDanhMuc");

                entity.HasIndex(e => e.MaThuongHieu, "IX_SanPham_MaThuongHieu");

                entity.HasIndex(e => e.MaXuatXu, "IX_SanPham_MaXuatXu");

                entity.HasIndex(e => e.TenSanPham, "UQ__SanPham__FCA8046945D8D7C3")
                    .IsUnique();

                entity.Property(e => e.GiaGoc).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.GiaKhuyenMai).HasColumnType("decimal(15, 2)");

                // Sửa từ HinhSanPham thành HinhSanPhamPath và ánh xạ với cột HinhSanPham trong database
                entity.Property(e => e.HinhSanPhamPath)
                    .HasColumnName("HinhSanPham") // Ánh xạ với cột HinhSanPham trong database
                    .HasMaxLength(255);


                entity.Property(e => e.NgayTao)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TenSanPham).HasMaxLength(200);

                entity.Property(e => e.TrangThai)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('HoatDong')");

                entity.HasOne(d => d.MaDanhMucNavigation)
                    .WithMany(p => p.SanPhams)
                    .HasForeignKey(d => d.MaDanhMuc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SanPham__MaDanhM__4316F928");

                entity.HasOne(d => d.MaThuongHieuNavigation)
                    .WithMany(p => p.SanPhams)
                    .HasForeignKey(d => d.MaThuongHieu)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SanPham__MaThuon__440B1D61");

                entity.HasOne(d => d.MaXuatXuNavigation)
                    .WithMany(p => p.SanPhams)
                    .HasForeignKey(d => d.MaXuatXu)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SanPham__MaXuatX__44FF419A");
            });

            modelBuilder.Entity<ThanhToan>(entity =>
            {
                entity.HasKey(e => e.MaThanhToan)
                    .HasName("PK__ThanhToa__D4B258446B676B0C");

                entity.ToTable("ThanhToan");

                entity.HasIndex(e => e.MaDonHang, "IX_ThanhToan_MaDonHang");

                entity.Property(e => e.GhiChu).HasMaxLength(200);

                entity.Property(e => e.MaTrangThaiThanhToan).HasDefaultValueSql("((1))");

                entity.Property(e => e.NgayThanhToan)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SoTien).HasColumnType("decimal(15, 2)");

                entity.HasOne(d => d.MaDonHangNavigation)
                    .WithMany(p => p.ThanhToans)
                    .HasForeignKey(d => d.MaDonHang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ThanhToan__MaDon__7C4F7684");

                entity.HasOne(d => d.MaPhuongThucThanhToanNavigation)
                    .WithMany(p => p.ThanhToans)
                    .HasForeignKey(d => d.MaPhuongThucThanhToan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ThanhToan__MaPhu__7D439ABD");

                entity.HasOne(d => d.MaTrangThaiThanhToanNavigation)
                    .WithMany(p => p.ThanhToans)
                    .HasForeignKey(d => d.MaTrangThaiThanhToan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ThanhToan__MaTra__7E37BEF6");
            });

            modelBuilder.Entity<ThongTinCuaHang>(entity =>
            {
                entity.HasKey(e => e.MaThongTin)
                    .HasName("PK__ThongTin__5E4CADDCAE92C965");

                entity.ToTable("ThongTinCuaHang");

                entity.Property(e => e.BanQuyen).HasMaxLength(255);

                entity.Property(e => e.DiaChi).HasMaxLength(255);

                entity.Property(e => e.DuongDanLogo).HasMaxLength(255);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.LienKetFacebook).HasMaxLength(255);

                entity.Property(e => e.LienKetInstagram).HasMaxLength(255);

                entity.Property(e => e.LienKetYoutube).HasMaxLength(255);

                entity.Property(e => e.NgayCapNhat)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SoDienThoai).HasMaxLength(20);

                entity.Property(e => e.TenCuaHang).HasMaxLength(100);
            });

            modelBuilder.Entity<ThuongHieu>(entity =>
            {
                entity.HasKey(e => e.MaThuongHieu)
                    .HasName("PK__ThuongHi__A3733E2CB7097A4F");

                entity.ToTable("ThuongHieu");

                entity.HasIndex(e => e.TenThuongHieu, "UQ__ThuongHi__98D6A83425BB2660")
                    .IsUnique();

                entity.Property(e => e.TenThuongHieu).HasMaxLength(100);
            });

            modelBuilder.Entity<TinTuc>(entity =>
            {
                entity.HasKey(e => e.MaTinTuc)
                    .HasName("PK__TinTuc__B53648C05499083C");

                entity.ToTable("TinTuc");

                entity.HasIndex(e => e.TieuDe, "UQ__TinTuc__371689AA6A6E94C5")
                    .IsUnique();

                entity.Property(e => e.HinhAnhDaiDien).HasMaxLength(255);

                entity.Property(e => e.NgayTao)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TieuDe).HasMaxLength(200);

                entity.Property(e => e.TrangThai)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.MaNguoiTaoNavigation)
                    .WithMany(p => p.TinTucs)
                    .HasForeignKey(d => d.MaNguoiTao)
                    .HasConstraintName("FK__TinTuc__MaNguoiT__123EB7A3");
            });

            modelBuilder.Entity<TrangThaiDonHang>(entity =>
            {
                entity.HasKey(e => e.MaTrangThaiDonHang)
                    .HasName("PK__TrangTha__B57A45F5C95A5F31");

                entity.ToTable("TrangThaiDonHang");

                entity.HasIndex(e => e.TenTrangThai, "UQ__TrangTha__9489EF66B80C87E7")
                    .IsUnique();

                entity.Property(e => e.MoTa).HasMaxLength(255);

                entity.Property(e => e.TenTrangThai).HasMaxLength(20);
            });

            modelBuilder.Entity<TrangThaiThanhToan>(entity =>
            {
                entity.HasKey(e => e.MaTrangThaiThanhToan)
                    .HasName("PK__TrangTha__A752C0880C10040E");

                entity.ToTable("TrangThaiThanhToan");

                entity.HasIndex(e => e.TenTrangThai, "UQ__TrangTha__9489EF6651D4152B")
                    .IsUnique();

                entity.Property(e => e.MoTa).HasMaxLength(255);

                entity.Property(e => e.TenTrangThai).HasMaxLength(20);
            });

            modelBuilder.Entity<VaiTro>(entity =>
            {
                entity.HasKey(e => e.MaVaiTro)
                    .HasName("PK__VaiTro__C24C41CF4ADB4984");

                entity.ToTable("VaiTro");

                entity.HasIndex(e => e.TenVaiTro, "UQ__VaiTro__1DA55814CBB70A2F")
                    .IsUnique();

                entity.Property(e => e.TenVaiTro).HasMaxLength(50);
            });

            modelBuilder.Entity<XacThucQuenMatKhau>(entity =>
            {
                entity.HasKey(e => e.MaXacThucQuenMatKhau)
                    .HasName("PK__XacThucQ__42BA9256636347A4");

                entity.ToTable("XacThucQuenMatKhau");

                entity.HasIndex(e => e.Token, "UQ__XacThucQ__1EB4F81754E7218D")
                    .IsUnique();

                entity.Property(e => e.DaSuDung).HasDefaultValueSql("((0))");

                entity.Property(e => e.ThoiGianHetHan).HasColumnType("datetime");

                entity.Property(e => e.Token).HasMaxLength(100);

                entity.HasOne(d => d.MaNguoiDungNavigation)
                    .WithMany(p => p.XacThucQuenMatKhaus)
                    .HasForeignKey(d => d.MaNguoiDung)
                    .HasConstraintName("FK__XacThucQu__MaNgu__17036CC0");
            });

            modelBuilder.Entity<XuatXu>(entity =>
            {
                entity.HasKey(e => e.MaXuatXu)
                    .HasName("PK__XuatXu__27BB6B19A7E11E13");

                entity.ToTable("XuatXu");

                entity.HasIndex(e => e.TenXuatXu, "UQ__XuatXu__95AC6057EC6CE4FF")
                    .IsUnique();

                entity.Property(e => e.TenXuatXu).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
