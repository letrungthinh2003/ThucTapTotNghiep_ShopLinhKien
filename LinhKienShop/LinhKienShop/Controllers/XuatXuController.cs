using LinhKienShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace LinhKienShop.Controllers
{
    public class XuatXuController : Controller
    {
        private readonly ShopLinhKienContext db;

        public XuatXuController()
        {
            db = new ShopLinhKienContext();
        }

        public IActionResult Index()
        {
            ViewBag.xx = db.XuatXus;
            return View();
        }

        // GET: Hiển thị form thêm danh mục
        public IActionResult Them()
        {
            return View();
        }

        // POST: Xử lý thêm danh mục
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Them(XuatXu xx)
        {
            if (ModelState.IsValid)
            {
                var existingxuatxu = await db.XuatXus
                    .FirstOrDefaultAsync(d => d.TenXuatXu.ToLower() == xx.TenXuatXu.ToLower());

                if (existingxuatxu != null)
                {
                    ModelState.AddModelError("TenXuatXu", "Tên xuất xứ này đã tồn tại, không được thêm.");
                    return View(xx);
                }

                db.Add(xx);
                await db.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm xuất xứ thành công!";
                return RedirectToAction("Index");
            }

            return View(xx);
        }

        // GET: Hiển thị form xác nhận xóa danh mục
        public async Task<IActionResult> Xoa(string id)
        {
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int MaXuatXU))
            {
                return NotFound();
            }

            var xuatxu = await db.XuatXus
                .FirstOrDefaultAsync(d => d.MaXuatXu == MaXuatXU);

            if (xuatxu == null)
            {
                return NotFound();
            }

            var productCount = await db.SanPhams
                .CountAsync(sp => sp.MaXuatXu == xuatxu.MaXuatXu);

            ViewBag.ProductCount = productCount;

            return View(xuatxu);
        }

        // POST: Xử lý xóa danh mục
        [HttpPost]
        [ActionName("Xoa")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int MaXuatXu))
            {
                return NotFound();
            }

            var xuatxu = await db.XuatXus
                .FirstOrDefaultAsync(d => d.MaXuatXu == MaXuatXu);

            if (xuatxu == null)
            {
                return NotFound();
            }

            var productCount = await db.SanPhams
                  .CountAsync(sp => sp.MaXuatXu == xuatxu.MaXuatXu);

            if (productCount > 0)
            {
                TempData["ErrorMessage"] = "Xuất xứ đang chứa sản phẩm, không thể xóa.";
                return RedirectToAction("Xoa", new { id });
            }

            db.XuatXus.Remove(xuatxu);
            await db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Xóa xuất xứ thành công!";
            return RedirectToAction("Index");
        }
        // GET: Hiển thị form sửa danh mục
        public async Task<IActionResult> Sua(string id)
        {
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int MaXuatXu))
            {
                return NotFound();
            }


            var xuatxu = await db.XuatXus
                .FirstOrDefaultAsync(d => d.MaXuatXu == MaXuatXu);

            if (xuatxu == null)
            {
                return NotFound();
            }

            return View(xuatxu);
        }

        // POST: Xử lý sửa danh mục
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sua(int maxuatxu, string tenxuatxu)
        {
            if (string.IsNullOrEmpty(tenxuatxu))
            {
                ModelState.AddModelError("TenXuatXu", "Tên xuất xứ không được để trống.");
                var xuatxuToEdit = await db.XuatXus.FindAsync(maxuatxu);
                return View(xuatxuToEdit);
            }

            var xuatxu = await db.XuatXus.FindAsync(maxuatxu);
            if (xuatxu == null)
            {
                return NotFound();
            }

            // Kiểm tra xem tên danh mục mới có trùng với danh mục khác không
            var existingXuatxu = await db.XuatXus
                .FirstOrDefaultAsync(d => d.TenXuatXu.ToLower() == tenxuatxu.ToLower()
                                       && d.MaXuatXu != maxuatxu);

            if (existingXuatxu != null)
            {
                ModelState.AddModelError("TenDanhMuc", "Tên danh mục này đã tồn tại, không được sửa.");
                return View(xuatxu);
            }

            // Cập nhật tên danh mục
            xuatxu.TenXuatXu = tenxuatxu;
            db.Update(xuatxu);
            await db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Sửa xuất xứ thành công!";
            return RedirectToAction("Index");
        }
    }
}
