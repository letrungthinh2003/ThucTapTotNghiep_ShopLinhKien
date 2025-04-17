using LinhKienShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;
using System.Globalization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace LinhKienShop.Controllers
{
    public class TrangChuController : Controller
    {
        private readonly ShopLinhKienContext _context;

        public TrangChuController(ShopLinhKienContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        private string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            string normalizedString = text.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public async Task<IActionResult> Index(string search = "", int? danhMucId = null, int? thuongHieuId = null, int? maxPrice = null, string sortOrder = "")
        {
            // Kiểm tra trạng thái đăng nhập và vai trò
            if (User.Identity.IsAuthenticated)
            {
                if (!Request.Cookies.TryGetValue("UserRole", out string roleValue) || !int.TryParse(roleValue, out int maVaiTro))
                {
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    Response.Cookies.Delete("UserRole");
                    return RedirectToAction("Login", "Account");
                }

                Console.WriteLine($"Khôi phục vai trò từ Cookie: MaVaiTro = {maVaiTro}");

                if (maVaiTro != 2)
                {
                    switch (maVaiTro)
                    {
                        case 1: return RedirectToAction("Index", "Dashboard"); // Admin
                        case 3: return RedirectToAction("Index", "NVQL"); // Nhân viên quản lý
                        case 4: return RedirectToAction("Index", "NVCSKH"); // Nhân viên CSKH
                        default: return RedirectToAction("Login", "Account"); // Vai trò không hợp lệ
                    }
                }
            }

            // Logic hiển thị trang chủ
            var sanPhams = _context.SanPhams.AsQueryable();

            // Lọc theo danh mục
            if (danhMucId.HasValue)
            {
                sanPhams = sanPhams.Where(s => s.MaDanhMuc == danhMucId);
            }
            // Lọc theo thương hiệu
            if (thuongHieuId.HasValue)
            {
                sanPhams = sanPhams.Where(s => s.MaThuongHieu == thuongHieuId);
            }
            // Lọc theo giá tối đa
            if (maxPrice.HasValue)
            {
                sanPhams = sanPhams.Where(s => s.GiaKhuyenMai <= maxPrice);
            }

            // Bao gồm các navigation properties
            sanPhams = sanPhams
                .Include(s => s.MaDanhMucNavigation)
                .Include(s => s.MaThuongHieuNavigation)
                .Include(s => s.MaXuatXuNavigation);

            // Sắp xếp theo giá
            switch (sortOrder)
            {
                case "price_asc":
                    sanPhams = sanPhams.OrderBy(s => s.GiaKhuyenMai);
                    break;
                case "price_desc":
                    sanPhams = sanPhams.OrderByDescending(s => s.GiaKhuyenMai);
                    break;
                default:
                    sanPhams = sanPhams.OrderBy(s => s.MaSanPham); // Mặc định sắp xếp theo MaSanPham
                    break;
            }

            var sanPhamList = await sanPhams.ToListAsync();

            // Tìm kiếm sản phẩm
            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim().ToLower();
                string searchNoDiacritics = RemoveDiacritics(search);
                sanPhamList = sanPhamList.Where(s => s.TenSanPham.ToLower().Contains(search) ||
                                                    RemoveDiacritics(s.TenSanPham).ToLower().Contains(searchNoDiacritics))
                                        .ToList();
                SaveSearchHistory(search);
            }

            // Truyền thông tin vào ViewBag
            ViewBag.DanhMucs = await _context.DanhMucs.ToListAsync() ?? new List<DanhMuc>();
            ViewBag.ThuongHieus = await _context.ThuongHieus.ToListAsync() ?? new List<ThuongHieu>();
            ViewBag.SearchHistory = GetSearchHistory();
            ViewBag.SelectedSortOrder = sortOrder; // Lưu trạng thái sắp xếp để hiển thị trong giao diện

            ViewData["Title"] = "Trang Chủ";
            return View(sanPhamList);
        }

        public async Task<IActionResult> Search(string search = "", int? danhMucId = null, int? thuongHieuId = null, int? maxPrice = null, string sortOrder = "")
        {
            var sanPhams = _context.SanPhams.AsQueryable();

            // Lọc theo danh mục
            if (danhMucId.HasValue)
            {
                sanPhams = sanPhams.Where(s => s.MaDanhMuc == danhMucId);
            }
            // Lọc theo thương hiệu
            if (thuongHieuId.HasValue)
            {
                sanPhams = sanPhams.Where(s => s.MaThuongHieu == thuongHieuId);
            }
            // Lọc theo giá tối đa
            if (maxPrice.HasValue)
            {
                sanPhams = sanPhams.Where(s => s.GiaKhuyenMai <= maxPrice);
            }

            // Bao gồm các navigation properties
            sanPhams = sanPhams
                .Include(s => s.MaDanhMucNavigation)
                .Include(s => s.MaThuongHieuNavigation)
                .Include(s => s.MaXuatXuNavigation);

            // Sắp xếp theo giá
            switch (sortOrder)
            {
                case "price_asc":
                    sanPhams = sanPhams.OrderBy(s => s.GiaKhuyenMai);
                    break;
                case "price_desc":
                    sanPhams = sanPhams.OrderByDescending(s => s.GiaKhuyenMai);
                    break;
                default:
                    sanPhams = sanPhams.OrderBy(s => s.MaSanPham); // Mặc định sắp xếp theo MaSanPham
                    break;
            }

            var sanPhamList = await sanPhams.ToListAsync();

            // Tìm kiếm sản phẩm
            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim().ToLower();
                string searchNoDiacritics = RemoveDiacritics(search);
                sanPhamList = sanPhamList.Where(s => s.TenSanPham.ToLower().Contains(search) ||
                                                    RemoveDiacritics(s.TenSanPham).ToLower().Contains(searchNoDiacritics))
                                        .ToList();
                SaveSearchHistory(search);
            }

            // Truyền thông tin vào ViewBag
            ViewBag.DanhMucs = await _context.DanhMucs.ToListAsync() ?? new List<DanhMuc>();
            ViewBag.ThuongHieus = await _context.ThuongHieus.ToListAsync() ?? new List<ThuongHieu>();
            ViewBag.SearchHistory = GetSearchHistory();
            ViewBag.SelectedSortOrder = sortOrder; // Lưu trạng thái sắp xếp để hiển thị trong giao diện

            ViewData["Title"] = "Kết Quả Tìm Kiếm";
            return View(sanPhamList);
        }

        private void SaveSearchHistory(string searchTerm)
        {
            var historyJson = HttpContext.Session.GetString("SearchHistory");
            var history = string.IsNullOrEmpty(historyJson)
                ? new List<string>()
                : JsonConvert.DeserializeObject<List<string>>(historyJson);

            if (!history.Contains(searchTerm))
            {
                history.Insert(0, searchTerm);
                if (history.Count > 5)
                {
                    history.RemoveAt(history.Count - 1);
                }
                HttpContext.Session.SetString("SearchHistory", JsonConvert.SerializeObject(history));
            }
        }

        private List<string> GetSearchHistory()
        {
            var historyJson = HttpContext.Session.GetString("SearchHistory");
            return string.IsNullOrEmpty(historyJson)
                ? new List<string>()
                : JsonConvert.DeserializeObject<List<string>>(historyJson);
        }

        public async Task<IActionResult> ChiTietSanPham(int? id)
        {
            if (id == null) return NotFound();

            var sanPham = await _context.SanPhams
                .Include(s => s.MaDanhMucNavigation)
                .Include(s => s.MaThuongHieuNavigation)
                .Include(s => s.MaXuatXuNavigation)
                .Include(s => s.HinhChiTietSliders)
                .FirstOrDefaultAsync(m => m.MaSanPham == id);

            if (sanPham == null) return NotFound();

            var similarProducts = await _context.SanPhams
                .Where(sp => sp.MaDanhMuc == sanPham.MaDanhMuc && sp.MaSanPham != sanPham.MaSanPham)
                .Take(8)
                .ToListAsync();

            ViewBag.SimilarProducts = similarProducts ?? new List<SanPham>();

            return View("ChiTietSanPham", sanPham);
        }
    }
}