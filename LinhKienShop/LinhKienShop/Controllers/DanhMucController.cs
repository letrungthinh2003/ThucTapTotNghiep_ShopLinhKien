using LinhKienShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LinhKienShop.Controllers
{
    public class DanhMucController : Controller
    {
        private readonly ShopLinhKienContext db;

        public DanhMucController(ShopLinhKienContext context)
        {
            db = context; // Sử dụng dependency injection thay vì khởi tạo trực tiếp
        }

        // GET: Index với phân trang
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 6; // Số danh mục mỗi trang
            var danhMucs = db.DanhMucs;

            // Tính tổng số danh mục và tổng số trang
            int totalItems = await danhMucs.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Lấy danh sách danh mục cho trang hiện tại
            var pagedDanhMucs = await danhMucs
                .OrderBy(d => d.MaDanhMuc) // Sắp xếp theo MaDanhMuc
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Truyền danh sách danh mục và thông tin phân trang qua ViewBag
            ViewBag.dm = pagedDanhMucs;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = page > 1;
            ViewBag.HasNext = page < totalPages;

            return View();
        }

        // GET: Thêm
        public IActionResult Them()
        {
            return View();
        }

        // POST: Thêm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Them(DanhMuc danhMuc)
        {
            if (ModelState.IsValid)
            {
                var existingDanhMuc = await db.DanhMucs
                    .FirstOrDefaultAsync(d => d.TenDanhMuc.ToLower() == danhMuc.TenDanhMuc.ToLower());

                if (existingDanhMuc != null)
                {
                    ModelState.AddModelError("TenDanhMuc", "Tên danh mục này đã tồn tại, không được thêm.");
                    return View(danhMuc);
                }

                db.Add(danhMuc);
                await db.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm danh mục thành công!";
                return RedirectToAction("Index");
            }
            return View(danhMuc);
        }

        // GET: Xóa
        public async Task<IActionResult> Xoa(string id)
        {
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int maDanhMuc))
            {
                return NotFound();
            }

            var danhMuc = await db.DanhMucs
                .FirstOrDefaultAsync(d => d.MaDanhMuc == maDanhMuc);

            if (danhMuc == null)
            {
                return NotFound();
            }

            var productCount = await db.SanPhams
                .CountAsync(sp => sp.MaDanhMuc == danhMuc.MaDanhMuc);

            ViewBag.ProductCount = productCount;

            return View(danhMuc);
        }

        // POST: Xóa
        [HttpPost]
        [ActionName("Xoa")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int maDanhMuc))
            {
                return NotFound();
            }

            var danhMuc = await db.DanhMucs
                .FirstOrDefaultAsync(d => d.MaDanhMuc == maDanhMuc);

            if (danhMuc == null)
            {
                return NotFound();
            }

            var productCount = await db.SanPhams
                .CountAsync(sp => sp.MaDanhMuc == danhMuc.MaDanhMuc);

            if (productCount > 0)
            {
                TempData["ErrorMessage"] = "Danh mục đang chứa sản phẩm, không thể xóa.";
                return RedirectToAction("Xoa", new { id });
            }

            db.DanhMucs.Remove(danhMuc);
            await db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Xóa danh mục thành công!";
            return RedirectToAction("Index");
        }

        // GET: Hiển thị form sửa danh mục
        public async Task<IActionResult> Sua(string id)
        {
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int maDanhMuc))
            {
                return NotFound();
            }

            var danhMuc = await db.DanhMucs
                .FirstOrDefaultAsync(d => d.MaDanhMuc == maDanhMuc);

            if (danhMuc == null)
            {
                return NotFound();
            }

            return View(danhMuc);
        }

        // POST: Xử lý sửa danh mục
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sua(int maDanhMuc, string tenDanhMuc)
        {
            if (string.IsNullOrEmpty(tenDanhMuc))
            {
                ModelState.AddModelError("tenDanhMuc", "Tên danh mục không được để trống.");
            }

            var danhMuc = await db.DanhMucs.FindAsync(maDanhMuc);
            if (danhMuc == null)
            {
                return NotFound();
            }

            var existingDanhMuc = await db.DanhMucs
                .FirstOrDefaultAsync(d => d.TenDanhMuc.ToLower() == tenDanhMuc.ToLower()
                                       && d.MaDanhMuc != maDanhMuc);

            if (existingDanhMuc != null)
            {
                ModelState.AddModelError("tenDanhMuc", "Tên danh mục này đã tồn tại, không được sửa.");
            }

            if (!ModelState.IsValid)
            {
                danhMuc.TenDanhMuc = tenDanhMuc; // Gán tạm để hiển thị lại trên form
                return View(danhMuc);
            }

            danhMuc.TenDanhMuc = tenDanhMuc;
            db.Update(danhMuc);
            await db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Sửa danh mục thành công!";
            return RedirectToAction("Index");
        }
    }
}