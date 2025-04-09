using LinhKienShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LinhKienShop.Controllers
{
    public class ThuongHieuController : Controller
    {
        private readonly ShopLinhKienContext db;

        public ThuongHieuController()
        {
            db = new ShopLinhKienContext();
        }

        // Hiển thị danh sách thương hiệu
        public IActionResult Index()
        {
            ViewBag.th = db.ThuongHieus;
            return View();
        }

        // GET: Hiển thị form thêm thương hiệu
        public IActionResult Them()
        {
            return View();
        }

        // POST: Xử lý thêm thương hiệu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Them(ThuongHieu th)
        {
            if (ModelState.IsValid)
            {
                var existingThuongHieu = await db.ThuongHieus
                    .FirstOrDefaultAsync(d => d.TenThuongHieu.ToLower() == th.TenThuongHieu.ToLower());

                if (existingThuongHieu != null)
                {
                    ModelState.AddModelError("TenThuongHieu", "Tên thương hiệu này đã tồn tại, không được thêm.");
                    return View(th);
                }

                db.Add(th);
                await db.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm thương hiệu thành công!";
                return RedirectToAction("Index");
            }
            return View(th);
        }

        // GET: Hiển thị form xác nhận xóa thương hiệu
        public async Task<IActionResult> Xoa(string id)
        {
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int maThuongHieu))
            {
                return NotFound();
            }

            var thuongHieu = await db.ThuongHieus
                .FirstOrDefaultAsync(d => d.MaThuongHieu == maThuongHieu);

            if (thuongHieu == null)
            {
                return NotFound();
            }

            var productCount = await db.SanPhams
                .CountAsync(sp => sp.MaThuongHieu == thuongHieu.MaThuongHieu);

            ViewBag.ProductCount = productCount;

            return View(thuongHieu);
        }

        // POST: Xử lý xóa thương hiệu
        [HttpPost]
        [ActionName("Xoa")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int maThuongHieu))
            {
                return NotFound();
            }

            var thuongHieu = await db.ThuongHieus
                .FirstOrDefaultAsync(d => d.MaThuongHieu == maThuongHieu);

            if (thuongHieu == null)
            {
                return NotFound();
            }

            var productCount = await db.SanPhams
                .CountAsync(sp => sp.MaThuongHieu == thuongHieu.MaThuongHieu);

            if (productCount > 0)
            {
                TempData["ErrorMessage"] = "Thương hiệu đang chứa sản phẩm, không thể xóa.";
                return RedirectToAction("Xoa", new { id });
            }

            db.ThuongHieus.Remove(thuongHieu);
            await db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Xóa thương hiệu thành công!";
            return RedirectToAction("Index");
        }

        // GET: Hiển thị form sửa thương hiệu
        public async Task<IActionResult> Sua(string id)
        {
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int maThuongHieu))
            {
                return NotFound();
            }

            var thuongHieu = await db.ThuongHieus
                .FirstOrDefaultAsync(d => d.MaThuongHieu == maThuongHieu);

            if (thuongHieu == null)
            {
                return NotFound();
            }

            return View(thuongHieu);
        }

        // POST: Xử lý sửa thương hiệu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sua(int maThuongHieu, string tenThuongHieu)
        {
            if (string.IsNullOrEmpty(tenThuongHieu))
            {
                ModelState.AddModelError("tenThuongHieu", "Tên thương hiệu không được để trống.");
            }

            var thuongHieu = await db.ThuongHieus.FindAsync(maThuongHieu);
            if (thuongHieu == null)
            {
                return NotFound();
            }

            var existingThuongHieu = await db.ThuongHieus
                .FirstOrDefaultAsync(d => d.TenThuongHieu.ToLower() == tenThuongHieu.ToLower()
                                       && d.MaThuongHieu != maThuongHieu);

            if (existingThuongHieu != null)
            {
                ModelState.AddModelError("tenThuongHieu", "Tên thương hiệu này đã tồn tại, không được sửa.");
            }

            if (!ModelState.IsValid)
            {
                thuongHieu.TenThuongHieu = tenThuongHieu;
                return View(thuongHieu);
            }

            thuongHieu.TenThuongHieu = tenThuongHieu;
            db.Update(thuongHieu);
            await db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Sửa thương hiệu thành công!";
            return RedirectToAction("Index");
        }
    }
}