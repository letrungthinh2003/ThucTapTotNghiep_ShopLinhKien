using LinhKienShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System.Text.RegularExpressions;

namespace LinhKienShop.Controllers
{
    public class NguoiDungController : Controller
    {
        private readonly ShopLinhKienContext _context;

        public NguoiDungController(ShopLinhKienContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var nguoiDungs = await _context.NguoiDungs
                .Include(n => n.MaVaiTroNavigation)
                .ToListAsync();
            return View(nguoiDungs);
        }

        public IActionResult Them()
        {
            ViewBag.VaiTros = _context.VaiTros.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Them(NguoiDung nguoiDung)
        {
            if (!string.IsNullOrEmpty(nguoiDung.SoDienThoai) && !IsValidPhoneNumber(nguoiDung.SoDienThoai))
            {
                ModelState.AddModelError("SoDienThoai", "Số điện thoại phải có 10 chữ số và đúng định dạng.");
            }

            if (!IsValidEmail(nguoiDung.Email))
            {
                ModelState.AddModelError("Email", "Email không đúng định dạng.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    nguoiDung.MatKhau = BCrypt.Net.BCrypt.HashPassword(nguoiDung.MatKhau);
                    nguoiDung.NgayTao = DateTime.Now;
                    nguoiDung.TrangThai = "HoatDong";

                    _context.Add(nguoiDung);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Thêm người dùng thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("UNIQUE"))
                    {
                        ModelState.AddModelError("Email", "Email đã tồn tại. Vui lòng sử dụng email khác.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Có lỗi xảy ra khi thêm người dùng. Vui lòng thử lại.");
                    }
                }
            }

            ViewBag.VaiTros = _context.VaiTros.ToList();
            return View(nguoiDung);
        }

        public async Task<IActionResult> Sua(int id)
        {
            var nguoiDung = await _context.NguoiDungs.FindAsync(id);
            if (nguoiDung == null) return NotFound();

            ViewBag.VaiTros = _context.VaiTros.ToList();
            return View(nguoiDung);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sua(int id, NguoiDung nguoiDung)
        {
            if (id != nguoiDung.MaNguoiDung) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _context.NguoiDungs.FindAsync(id);
                    if (existingUser == null) return NotFound();

                    existingUser.Email = nguoiDung.Email;
                    existingUser.HoTen = nguoiDung.HoTen;
                    existingUser.SoDienThoai = nguoiDung.SoDienThoai;
                    existingUser.DiaChi = nguoiDung.DiaChi;
                    existingUser.MaVaiTro = nguoiDung.MaVaiTro;
                    existingUser.TrangThai = nguoiDung.TrangThai;

                    if (!string.IsNullOrEmpty(nguoiDung.MatKhau))
                    {
                        existingUser.MatKhau = BCrypt.Net.BCrypt.HashPassword(nguoiDung.MatKhau);
                    }

                    _context.Update(existingUser);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Sửa người dùng thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.NguoiDungs.Any(e => e.MaNguoiDung == id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.VaiTros = _context.VaiTros.ToList();
            return View(nguoiDung);
        }

        public async Task<IActionResult> Xoa(int id)
        {
            var nguoiDung = await _context.NguoiDungs
                .Include(n => n.MaVaiTroNavigation)
                .FirstOrDefaultAsync(n => n.MaNguoiDung == id);
            if (nguoiDung == null) return NotFound();

            return View(nguoiDung);
        }

        [HttpPost, ActionName("Xoa")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaConfirmed(int id)
        {
            var nguoiDung = await _context.NguoiDungs.FindAsync(id);
            if (nguoiDung == null) return NotFound();

            _context.NguoiDungs.Remove(nguoiDung);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Xóa người dùng thành công!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleTrangThai(int id)
        {
            var nguoiDung = await _context.NguoiDungs.FindAsync(id);
            if (nguoiDung == null) return NotFound();

            nguoiDung.TrangThai = nguoiDung.TrangThai == "HoatDong" ? "BiKhoa" : "HoatDong";
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Cập nhật trạng thái thành công!";
            return RedirectToAction(nameof(Index));
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private bool IsValidPhoneNumber(string phone)
        {
            return Regex.IsMatch(phone, @"^[0-9]{10}$");
        }
    }
}