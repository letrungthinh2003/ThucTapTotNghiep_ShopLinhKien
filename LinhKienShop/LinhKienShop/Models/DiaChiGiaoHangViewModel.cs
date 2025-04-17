namespace LinhKienShop.Models
{
    public class DiaChiGiaoHangViewModel
    {
        public int MaDiaChiGiaoHang { get; set; }
        public int MaNguoiDung { get; set; }
        public string HoTenNguoiNhan { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChiChiTiet { get; set; }
        public string ThanhPho { get; set; }
        public string QuanHuyen { get; set; }
        public string PhuongXa { get; set; }
        public bool LaMacDinh { get; set; } // Non-nullable
        public DateTime? NgayTao { get; set; }
    }
}
