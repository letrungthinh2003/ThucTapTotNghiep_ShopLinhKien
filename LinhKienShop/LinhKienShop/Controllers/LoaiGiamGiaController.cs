using LinhKienShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LinhKienShop.Controllers
{
    public class LoaiGiamGiaController : Controller
    {
        private readonly ShopLinhKienContext _db;

        public LoaiGiamGiaController(ShopLinhKienContext db)
        {
            _db = db;
        }

        // GET: Hiển thị danh sách loại giảm giá
        public IActionResult Index()
        {
            var LoaiGiamGia = _db.LoaiGiamGia.ToList(); // Lấy danh sách loại giảm giá
            ViewBag.lgg = LoaiGiamGia; // Truyền danh sách vào ViewBag
            return View();
        }

        // GET: Hiển thị form thêm loại giảm giá
        public IActionResult Them()
        {
            return View();
        }

        // POST: Xử lý thêm loại giảm giá
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Them(LoaiGiamGium lgg)
        {
            if (ModelState.IsValid)
            {
                var existingLoaiGiamGia = await _db.LoaiGiamGia
                    .FirstOrDefaultAsync(d => d.TenLoaiGiamGia.ToLower() == lgg.TenLoaiGiamGia.ToLower());

                if (existingLoaiGiamGia != null)
                {
                    ModelState.AddModelError("TenLoaiGiamGia", "Tên loại giảm giá này đã tồn tại.");
                    return View(lgg);
                }

                _db.Add(lgg);
                await _db.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm loại giảm giá thành công!";
                return RedirectToAction("Index");
            }

            return View(lgg);
        }

        // GET: Hiển thị form xác nhận xóa loại giảm giá
        public async Task<IActionResult> Xoa(string id)
        {
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int maLoaiGiamGia))
            {
                return NotFound();
            }

            var loaiGiamGia = await _db.LoaiGiamGia
                .FirstOrDefaultAsync(d => d.MaLoaiGiamGia == maLoaiGiamGia);

            if (loaiGiamGia == null)
            {
                return NotFound();
            }

            // Kiểm tra xem loại giảm giá có đang được sử dụng trong bảng MaGiamGia không
            var discountCount = await _db.MaGiamGia
                .CountAsync(m => m.MaLoaiGiamGia == maLoaiGiamGia);
            ViewBag.DiscountCount = discountCount;

            return View(loaiGiamGia);
        }

        // POST: Xử lý xóa loại giảm giá
        [HttpPost, ActionName("Xoa")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int maLoaiGiamGia))
            {
                return NotFound();
            }

            var loaiGiamGia = await _db.LoaiGiamGia
                .FirstOrDefaultAsync(d => d.MaLoaiGiamGia == maLoaiGiamGia);

            if (loaiGiamGia == null)
            {
                return NotFound();
            }

            // Kiểm tra nếu loại giảm giá đang được sử dụng
            var discountCount = await _db.MaGiamGia
                .CountAsync(m => m.MaLoaiGiamGia == maLoaiGiamGia);
            if (discountCount > 0)
            {
                TempData["ErrorMessage"] = "Loại giảm giá đang được sử dụng trong mã giảm giá, không thể xóa.";
                return RedirectToAction("Xoa", new { id });
            }

            _db.LoaiGiamGia.Remove(loaiGiamGia);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Xóa loại giảm giá thành công!";
            return RedirectToAction("Index");
        }

        // GET: Hiển thị form sửa loại giảm giá
        public async Task<IActionResult> Sua(string id)
        {
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int maLoaiGiamGia))
            {
                return NotFound();
            }

            var loaiGiamGia = await _db.LoaiGiamGia
                .FirstOrDefaultAsync(d => d.MaLoaiGiamGia == maLoaiGiamGia);

            if (loaiGiamGia == null)
            {
                return NotFound();
            }

            return View(loaiGiamGia);
        }

        // POST: Xử lý sửa loại giảm giá
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sua(int maLoaiGiamGia, LoaiGiamGium loaiGiamGia)
        {
            if (maLoaiGiamGia != loaiGiamGia.MaLoaiGiamGia)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingLoaiGiamGia = await _db.LoaiGiamGia
                    .FirstOrDefaultAsync(d => d.TenLoaiGiamGia.ToLower() == loaiGiamGia.TenLoaiGiamGia.ToLower()
                                           && d.MaLoaiGiamGia != maLoaiGiamGia);

                if (existingLoaiGiamGia != null)
                {
                    ModelState.AddModelError("TenLoaiGiamGia", "Tên loại giảm giá này đã exist.");
                    return View(loaiGiamGia);
                }

                _db.Update(loaiGiamGia);
                await _db.SaveChangesAsync();
                TempData["SuccessMessage"] = "Sửa loại giảm giá thành công!";
                return RedirectToAction("Index");
            }

            return View(loaiGiamGia);
        }
    }
}