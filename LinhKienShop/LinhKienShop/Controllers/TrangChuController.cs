using LinhKienShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LinhKienShop.Controllers
{
    public class TrangChuController : Controller
    {
        private readonly ShopLinhKienContext _context;

        public TrangChuController(ShopLinhKienContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IActionResult> Index(string search = "", int? danhMucId = null, int? thuongHieuId = null, int? maxPrice = null)
        {
            var sanPhams = _context.SanPhams.AsQueryable();

            // Áp dụng các điều kiện lọc
            if (!string.IsNullOrEmpty(search))
            {
                sanPhams = sanPhams.Where(s => s.TenSanPham.Contains(search));
            }
            if (danhMucId.HasValue)
            {
                sanPhams = sanPhams.Where(s => s.MaDanhMuc == danhMucId);
            }
            if (thuongHieuId.HasValue)
            {
                sanPhams = sanPhams.Where(s => s.MaThuongHieu == thuongHieuId);
            }
            if (maxPrice.HasValue)
            {
                sanPhams = sanPhams.Where(s => s.GiaKhuyenMai <= maxPrice);
            }

            // Gọi Include sau khi lọc
            sanPhams = sanPhams
                .Include(s => s.MaDanhMucNavigation)
                .Include(s => s.MaThuongHieuNavigation)
                .Include(s => s.MaXuatXuNavigation);

            // Lấy tất cả sản phẩm
            var allSanPhams = await sanPhams
                .OrderBy(s => s.MaSanPham)
                .ToListAsync() ?? new List<SanPham>();

            // Truyền dữ liệu qua ViewBag
            ViewBag.DanhMucs = await _context.DanhMucs.ToListAsync() ?? new List<DanhMuc>();
            ViewBag.ThuongHieus = await _context.ThuongHieus.ToListAsync() ?? new List<ThuongHieu>();

            ViewData["Title"] = "Trang Chủ";
            return View(allSanPhams);
        }

        // GET: Chi tiết sản phẩm
        public async Task<IActionResult> ChiTietSanPham(int? id)
        {
            if (id == null) return NotFound();

            // Lấy thông tin sản phẩm chi tiết
            var sanPham = await _context.SanPhams
                .Include(s => s.MaDanhMucNavigation)
                .Include(s => s.MaThuongHieuNavigation)
                .Include(s => s.MaXuatXuNavigation)
                .Include(s => s.HinhChiTietSliders) // Load danh sách hình ảnh chi tiết
                .FirstOrDefaultAsync(m => m.MaSanPham == id);

            if (sanPham == null) return NotFound();

            // Lấy danh sách sản phẩm tương tự
            var similarProducts = await _context.SanPhams
                .Where(sp => sp.MaDanhMuc == sanPham.MaDanhMuc && sp.MaSanPham != sanPham.MaSanPham)
                .Take(8)
                .ToListAsync();

            // Truyền danh sách sản phẩm tương tự qua ViewBag
            ViewBag.SimilarProducts = similarProducts ?? new List<SanPham>();

            return View("ChiTietSanPham", sanPham); // Trả về view ChiTietSanPham.cshtml
        }
    }
}