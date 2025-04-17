using LinhKienShop.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using BCrypt.Net;

namespace LinhKienShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly ShopLinhKienContext _context;

        public AccountController(ShopLinhKienContext context)
        {
            _context = context;
        }

        // GET: /dang-ky
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        // POST: /dang-ky
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                if (await _context.NguoiDungs.AnyAsync(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email đã được sử dụng.");
                    return View(model);
                }

                var nguoiDung = new NguoiDung
                {
                    HoTen = model.HoTen,
                    Email = model.Email,
                    MatKhau = BCrypt.Net.BCrypt.HashPassword(model.MatKhau),
                    SoDienThoai = model.SoDienThoai,
                    DiaChi = model.DiaChi,
                    MaVaiTro = 2, // Vai trò mặc định là khách hàng
                    NgayTao = DateTime.Now,
                    TrangThai = "HoatDong"
                };

                _context.NguoiDungs.Add(nguoiDung);
                await _context.SaveChangesAsync();

                // Xóa TempData["SuccessMessage"] để không hiển thị thông báo
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi khi đăng ký. Vui lòng thử lại.");
                return View(model);
            }
        }

        // GET: /account/index - Hiển thị danh sách người dùng
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _context.NguoiDungs
                .Include(u => u.MaVaiTroNavigation)
                .ToListAsync();
            return View(users);
        }

        // GET: /dang-nhap
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        // POST: /dang-nhap
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _context.NguoiDungs
                .Include(u => u.MaVaiTroNavigation)
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.MatKhau, user.MatKhau))
            {
                ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
                return View(model);
            }

            if (user.TrangThai == "BiKhoa")
            {
                ModelState.AddModelError("", "Tài khoản của bạn đã bị khóa.");
                return View(model);
            }

            // Tạo cookie xác thực
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.HoTen),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.MaNguoiDung.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Lưu vai trò vào cookie riêng
            Response.Cookies.Append("UserRole", user.MaVaiTro.ToString(), new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.UtcNow.AddHours(1),
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return RedirectToRolePage(user.MaVaiTro);
        }

        // GET: /dang-xuat
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete("UserRole");
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            return RedirectToAction("Index", "TrangChu");
        }

        // POST: /toggle-trang-thai
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleTrangThai(int id)
        {
            var nguoiDung = await _context.NguoiDungs.FindAsync(id);
            if (nguoiDung == null)
            {
                return NotFound();
            }

            nguoiDung.TrangThai = nguoiDung.TrangThai == "HoatDong" ? "BiKhoa" : "HoatDong";
            _context.Update(nguoiDung);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private IActionResult RedirectToRolePage(int maVaiTro)
        {
            switch (maVaiTro)
            {
                case 1: return RedirectToAction("Index", "Dashboard"); // Admin
                case 2: return RedirectToAction("Index", "TrangChu"); // Khách hàng
                case 3: return RedirectToAction("Index", "NVQL"); // Nhân viên quản lý
                case 4: return RedirectToAction("Index", "NVCSKH"); // Nhân viên CSKH
                default: return RedirectToAction("Index", "TrangChu");
            }
        }
    }
}