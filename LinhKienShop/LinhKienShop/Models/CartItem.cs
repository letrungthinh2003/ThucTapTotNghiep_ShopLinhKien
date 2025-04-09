namespace LinhKienShop.Models
{
    public class CartItem
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string HinhSanPhamPath { get; set; }
        public decimal GiaKhuyenMai { get; set; }
        public int SoLuong { get; set; }
        public int SoLuongTonKho { get; set; } // Số lượng tồn kho tối đa
        public decimal ThanhTien => GiaKhuyenMai * SoLuong;
    }
}
