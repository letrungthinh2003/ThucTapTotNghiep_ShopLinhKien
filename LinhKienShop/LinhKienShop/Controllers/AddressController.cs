using LinhKienShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace LinhKienShop.Controllers
{
    [Authorize] // Yêu cầu người dùng phải đăng nhập
    public class AddressController : Controller
    {
        private readonly ShopLinhKienContext _context; // DbContext của ứng dụng
        private readonly IHttpClientFactory _httpClientFactory; // Sử dụng IHttpClientFactory thay vì HttpClient

        public AddressController(ShopLinhKienContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        // GET: Address/Index
        // GET: Address/Index
        public async Task<IActionResult> Index()
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account"); // Chuyển hướng đến trang đăng nhập
            }

            // Lấy ID người dùng từ thông tin đăng nhập
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                // Nếu không lấy được userId hợp lệ, chuyển hướng hoặc xử lý lỗi
                TempData["ErrorMessage"] = "Không thể xác định thông tin người dùng.";
                return RedirectToAction("Login", "Account");
            }

            // Lấy danh sách địa chỉ giao hàng của người dùng và ánh xạ sang ViewModel
            var addresses = await _context.DiaChiGiaoHangs
                .Where(a => a.MaNguoiDung == userId)
                .Select(a => new DiaChiGiaoHangViewModel
                {
                    MaDiaChiGiaoHang = a.MaDiaChiGiaoHang,
                    MaNguoiDung = a.MaNguoiDung,
                    HoTenNguoiNhan = a.HoTenNguoiNhan,
                    SoDienThoai = a.SoDienThoai,
                    DiaChiChiTiet = a.DiaChiChiTiet,
                    ThanhPho = a.ThanhPho,
                    QuanHuyen = a.QuanHuyen,
                    PhuongXa = a.PhuongXa,
                    LaMacDinh = a.LaMacDinh,
                    NgayTao = a.NgayTao
                })
                .ToListAsync();

            return View(addresses);
        }

        // GET: Address/Create
        public IActionResult Them()
        {
            return View(new DiaChiGiaoHangViewModel());
        }

        // POST: Address/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Them(DiaChiGiaoHangViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    TempData["ErrorMessage"] = "Không thể xác định thông tin người dùng.";
                    return RedirectToAction("Login", "Account");
                }

                var address = new DiaChiGiaoHang
                {
                    MaNguoiDung = userId,
                    HoTenNguoiNhan = model.HoTenNguoiNhan,
                    SoDienThoai = model.SoDienThoai,
                    DiaChiChiTiet = model.DiaChiChiTiet,
                    ThanhPho = model.ThanhPho,
                    QuanHuyen = model.QuanHuyen,
                    PhuongXa = model.PhuongXa,
                    LaMacDinh = model.LaMacDinh, // Bây giờ cả hai đều là bool, không cần cast
                    NgayTao = DateTime.Now
                };

                // Nếu địa chỉ mới là mặc định, đặt các địa chỉ khác thành không mặc định
                if (address.LaMacDinh)
                {
                    var existingAddresses = await _context.DiaChiGiaoHangs
                        .Where(a => a.MaNguoiDung == address.MaNguoiDung)
                        .ToListAsync();
                    foreach (var addr in existingAddresses)
                    {
                        addr.LaMacDinh = false;
                    }
                }

                _context.Add(address);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm địa chỉ giao hàng thành công!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

       
        // GET: Address/Edit/5
        public async Task<IActionResult> Sua(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Kiểm tra đăng nhập
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                TempData["ErrorMessage"] = "Không thể xác định thông tin người dùng.";
                return RedirectToAction("Login", "Account");
            }

            var address = await _context.DiaChiGiaoHangs
                .Where(a => a.MaDiaChiGiaoHang == id && a.MaNguoiDung == userId)
                .Select(a => new DiaChiGiaoHangViewModel
                {
                    MaDiaChiGiaoHang = a.MaDiaChiGiaoHang,
                    MaNguoiDung = a.MaNguoiDung,
                    HoTenNguoiNhan = a.HoTenNguoiNhan,
                    SoDienThoai = a.SoDienThoai,
                    DiaChiChiTiet = a.DiaChiChiTiet,
                    ThanhPho = a.ThanhPho,
                    QuanHuyen = a.QuanHuyen,
                    PhuongXa = a.PhuongXa,
                    LaMacDinh = a.LaMacDinh, // Bây giờ cả hai đều là bool
                    NgayTao = a.NgayTao
                })
                .FirstOrDefaultAsync();

            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // POST: Address/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sua(int id, DiaChiGiaoHangViewModel model)
        {
            if (id != model.MaDiaChiGiaoHang)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra đăng nhập
                    if (!User.Identity.IsAuthenticated)
                    {
                        return RedirectToAction("Login", "Account");
                    }

                    var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                    {
                        TempData["ErrorMessage"] = "Không thể xác định thông tin người dùng.";
                        return RedirectToAction("Login", "Account");
                    }

                    // Tìm địa chỉ cần cập nhật
                    var address = await _context.DiaChiGiaoHangs
                        .FirstOrDefaultAsync(a => a.MaDiaChiGiaoHang == id && a.MaNguoiDung == userId);

                    if (address == null)
                    {
                        return NotFound();
                    }

                    // Cập nhật các thuộc tính từ ViewModel
                    address.HoTenNguoiNhan = model.HoTenNguoiNhan;
                    address.SoDienThoai = model.SoDienThoai;
                    address.DiaChiChiTiet = model.DiaChiChiTiet;
                    address.ThanhPho = model.ThanhPho;
                    address.QuanHuyen = model.QuanHuyen;
                    address.PhuongXa = model.PhuongXa;
                    address.LaMacDinh = model.LaMacDinh; // Bây giờ cả hai đều là bool

                    // Nếu địa chỉ này được chọn là mặc định, đặt các địa chỉ khác thành không mặc định
                    if (model.LaMacDinh)
                    {
                        var existingAddresses = await _context.DiaChiGiaoHangs
                            .Where(a => a.MaNguoiDung == userId && a.MaDiaChiGiaoHang != id)
                            .ToListAsync();
                        foreach (var addr in existingAddresses)
                        {
                            addr.LaMacDinh = false;
                        }
                    }

                    _context.Update(address);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật địa chỉ giao hàng thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiaChiGiaoHangExists(model.MaDiaChiGiaoHang))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // POST: Address/Delete/5
        [HttpPost, ActionName("Xoa")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaConfirmed(int id)
        {
            // Kiểm tra đăng nhập
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                TempData["ErrorMessage"] = "Không thể xác định thông tin người dùng.";
                return RedirectToAction("Login", "Account");
            }

            var address = await _context.DiaChiGiaoHangs.FindAsync(id);
            if (address == null || address.MaNguoiDung != userId)
            {
                return NotFound();
            }

            _context.DiaChiGiaoHangs.Remove(address);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Xóa địa chỉ giao hàng thành công!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Address/GetProvinces
        [HttpGet]
        public async Task<IActionResult> GetProvinces()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("https://provinces.open-api.vn/api/p/");
                response.EnsureSuccessStatusCode(); // Ném ngoại lệ nếu không thành công
                var provinces = await response.Content.ReadAsStringAsync();
                return Content(provinces, "application/json");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Lỗi khi gọi API tỉnh/thành phố: {ex.Message}");
                return StatusCode(500, new { error = "Không thể lấy danh sách tỉnh/thành phố.", details = ex.Message });
            }
        }

        // GET: Address/GetDistricts?provinceCode={code}
        [HttpGet]
        public async Task<IActionResult> GetDistricts(int provinceCode)
        {
            if (provinceCode <= 0)
            {
                return BadRequest(new { error = "Mã tỉnh/thành phố không hợp lệ." });
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"https://provinces.open-api.vn/api/p/{provinceCode}?depth=2");
                response.EnsureSuccessStatusCode();
                var districts = await response.Content.ReadAsStringAsync();
                return Content(districts, "application/json");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Lỗi khi gọi API quận/huyện: {ex.Message}");
                return StatusCode(500, new { error = "Không thể lấy danh sách quận/huyện.", details = ex.Message });
            }
        }

        // GET: Address/GetWards?districtCode={code}
        [HttpGet]
        public async Task<IActionResult> GetWards(int districtCode)
        {
            if (districtCode <= 0)
            {
                return BadRequest(new { error = "Mã quận/huyện không hợp lệ." });
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"https://provinces.open-api.vn/api/d/{districtCode}?depth=2");
                response.EnsureSuccessStatusCode();
                var wards = await response.Content.ReadAsStringAsync();
                return Content(wards, "application/json");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Lỗi khi gọi API phường/xã: {ex.Message}");
                return StatusCode(500, new { error = "Không thể lấy danh sách phường/xã.", details = ex.Message });
            }
        }

        private bool DiaChiGiaoHangExists(int id)
        {
            return _context.DiaChiGiaoHangs.Any(e => e.MaDiaChiGiaoHang == id);
        }
    }
}