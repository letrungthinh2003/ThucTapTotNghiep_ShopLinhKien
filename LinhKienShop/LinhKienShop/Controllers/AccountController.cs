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
            Console.WriteLine("Register GET action called");
            return View();
        }

        // POST: /dang-ky
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(NguoiDung model)
        {
            Console.WriteLine($"MatKhau received: '{model.MatKhau}'");
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"Key: {error.Key}, Error: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                return View(model);
            }

            if (await _context.NguoiDungs.AnyAsync(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email đã được sử dụng.");
                return View(model);
            }

            model.MatKhau = BCrypt.Net.BCrypt.HashPassword(model.MatKhau);
            model.MaVaiTro = 2; // Vai trò mặc định là khách hàng
            model.NgayTao = DateTime.Now;
            model.TrangThai = "HoatDong";

            _context.NguoiDungs.Add(model);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đăng ký tài khoản thành công!";
            return RedirectToAction("Login");
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
            return View();
        }

        // POST: /dang-nhap
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string matKhau)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(matKhau))
            {
                ModelState.AddModelError("", "Email và mật khẩu không được để trống.");
                return View();
            }

            var user = await _context.NguoiDungs
                .Include(u => u.MaVaiTroNavigation)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user != null && BCrypt.Net.BCrypt.Verify(matKhau, user.MatKhau))
            {
                if (user.TrangThai == "BiKhoa")
                {
                    ModelState.AddModelError("", "Tài khoản của bạn đã bị khóa.");
                    return View();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.HoTen),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.MaVaiTroNavigation.TenVaiTro),
                    new Claim("MaVaiTro", user.MaVaiTro.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Redirect theo vai trò
                switch (user.MaVaiTro)
                {
                    case 1: return RedirectToAction("Index", "Dashboard"); // Admin
                    case 2: return RedirectToAction("Index", "TrangChu"); // Khách hàng
                    case 3: return RedirectToAction("Index", "NVQL"); // Nhân viên quản lý
                    case 4: return RedirectToAction("Index", "NVCSKH"); // Nhân viên CSKH
                    default: return RedirectToAction("Index", "TrangChu");
                }
            }

            ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
            return View();
        }

        // GET: /dang-xuat
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
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
    }
}