using LinhKienShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LinhKienShop.Controllers
{
    public class PhuongThucThanhToanController : Controller
    {
        private readonly ShopLinhKienContext _context;

        public PhuongThucThanhToanController(ShopLinhKienContext context)
        {
            _context = context;
        }

        // GET: PhuongThucThanhToan/Index
        public async Task<IActionResult> Index()
        {
            var phuongThucThanhToans = await _context.PhuongThucThanhToans.ToListAsync();
            ViewBag.pttt = phuongThucThanhToans;
            return View();
        }

        // GET: PhuongThucThanhToan/Them
        public IActionResult Them()
        {
            return View();
        }

        // POST: PhuongThucThanhToan/Them
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Them(PhuongThucThanhToan phuongThucThanhToan)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra trùng tên phương thức thanh toán
                if (await _context.PhuongThucThanhToans.AnyAsync(p => p.TenPhuongThucThanhToan == phuongThucThanhToan.TenPhuongThucThanhToan))
                {
                    ModelState.AddModelError("TenPhuongThucThanhToan", "Tên phương thức thanh toán đã tồn tại.");
                    return View(phuongThucThanhToan);
                }

                _context.Add(phuongThucThanhToan);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm phương thức thanh toán thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(phuongThucThanhToan);
        }

        // GET: PhuongThucThanhToan/Sua/5
        public async Task<IActionResult> Sua(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phuongThucThanhToan = await _context.PhuongThucThanhToans.FindAsync(id);
            if (phuongThucThanhToan == null)
            {
                return NotFound();
            }
            return View(phuongThucThanhToan);
        }

        // POST: PhuongThucThanhToan/Sua/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sua(int id, PhuongThucThanhToan phuongThucThanhToan)
        {
            if (id != phuongThucThanhToan.MaPhuongThucThanhToan)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Kiểm tra trùng tên phương thức thanh toán (ngoại trừ bản ghi hiện tại)
                if (await _context.PhuongThucThanhToans.AnyAsync(p => p.TenPhuongThucThanhToan == phuongThucThanhToan.TenPhuongThucThanhToan && p.MaPhuongThucThanhToan != id))
                {
                    ModelState.AddModelError("TenPhuongThucThanhToan", "Tên phương thức thanh toán đã tồn tại.");
                    return View(phuongThucThanhToan);
                }

                try
                {
                    _context.Update(phuongThucThanhToan);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Sửa phương thức thanh toán thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await PhuongThucThanhToanExists(id))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }
            return View(phuongThucThanhToan);
        }

        // GET: PhuongThucThanhToan/Xoa/5
        public async Task<IActionResult> Xoa(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phuongThucThanhToan = await _context.PhuongThucThanhToans
                .Include(p => p.DonHangs)
                .FirstOrDefaultAsync(m => m.MaPhuongThucThanhToan == id);
            if (phuongThucThanhToan == null)
            {
                return NotFound();
            }

            // Kiểm tra xem phương thức thanh toán có đang được sử dụng bởi đơn hàng
            if (phuongThucThanhToan.DonHangs.Any())
            {
                TempData["ErrorMessage"] = "Không thể xóa phương thức thanh toán vì đang được sử dụng trong đơn hàng.";
                return RedirectToAction(nameof(Index));
            }

            return View(phuongThucThanhToan);
        }

        // POST: PhuongThucThanhToan/Xoa/5
        [HttpPost, ActionName("Xoa")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaConfirmed(int id)
        {
            var phuongThucThanhToan = await _context.PhuongThucThanhToans.FindAsync(id);
            if (phuongThucThanhToan != null)
            {
                _context.PhuongThucThanhToans.Remove(phuongThucThanhToan);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa phương thức thanh toán thành công!";
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> PhuongThucThanhToanExists(int id)
        {
            return await _context.PhuongThucThanhToans.AnyAsync(e => e.MaPhuongThucThanhToan == id);
        }
    }
}