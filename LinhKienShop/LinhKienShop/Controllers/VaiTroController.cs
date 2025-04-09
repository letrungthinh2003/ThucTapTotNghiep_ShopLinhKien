using LinhKienShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LinhKienShop.Controllers
{
    public class VaiTroController : Controller
    {
        private readonly ShopLinhKienContext db;

        public VaiTroController(ShopLinhKienContext context)
        {
            db = context; // Sử dụng dependency injection thay vì khởi tạo trực tiếp
        }
        public IActionResult Index()
        {
            ViewBag.vt = db.VaiTros;
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
        public async Task<IActionResult> Them(VaiTro vt)
        {
            if (ModelState.IsValid)
            {
                var existingVaiTro = await db.VaiTros
                    .FirstOrDefaultAsync(d => d.TenVaiTro.ToLower() == vt.TenVaiTro.ToLower());

                if (existingVaiTro != null)
                {
                    ModelState.AddModelError("TenVaiTro", "Tên vai trò này đã tồn tại, không được thêm.");
                    return View(vt);
                }

                db.Add(vt);
                await db.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm vai trò mới thành công!";
                return RedirectToAction("Index");
            }
            return View(vt);
        }

        // GET: Xóa
        public async Task<IActionResult> Xoa(string id)
        {
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int maVaiTro))
            {
                return NotFound();
            }

            var vt = await db.VaiTros
                .FirstOrDefaultAsync(d => d.MaVaiTro == maVaiTro);


            if (vt == null)
            {
                return NotFound();
            }
            var NguoidungCount = await db.NguoiDungs.CountAsync(nd => nd.MaNguoiDung == vt.MaVaiTro);

            ViewBag.NguoidungCount = NguoidungCount;

            return View(vt);
        }

        // POST: Xóa
        [HttpPost]
        [ActionName("Xoa")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int maVaiTro))
            {
                return NotFound();
            }

            var vt = await db.VaiTros
                .FirstOrDefaultAsync(d => d.MaVaiTro == maVaiTro);

            if (vt == null)
            {
                return NotFound();
            }
            var NguoidungCount = await db.NguoiDungs.CountAsync(nd => nd.MaNguoiDung == vt.MaVaiTro);

            if (NguoidungCount > 0)
            {
                TempData["ErrorMessage"] = "Vai trò này đang chứa sản người dùng, không thể xóa.";
                return RedirectToAction("Xoa", new { id });
            }

            db.VaiTros.Remove(vt);
            await db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Xóa vai trò thành công!";
            return RedirectToAction("Index");
        }

        // GET: Hiển thị form sửa danh mục
        public async Task<IActionResult> Sua(string id)
        {
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int maVaiTro))
            {
                return NotFound();
            }

            var vt = await db.VaiTros
                .FirstOrDefaultAsync(d => d.MaVaiTro == maVaiTro);

            if (vt == null)
            {
                return NotFound();
            }

            return View(vt);
        }

        // POST: Xử lý sửa danh mục
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sua(int maVaiTro, string tenVaiTro)
        {
            if (string.IsNullOrEmpty(tenVaiTro))
            {
                ModelState.AddModelError("tenVaiTro", "Tên vai trò không được để trống.");
            }

            var vt = await db.VaiTros.FindAsync(maVaiTro);
            if (vt == null)
            {
                return NotFound();
            }

            var existingVaiTro= await db.VaiTros
                .FirstOrDefaultAsync(d => d.TenVaiTro.ToLower() == tenVaiTro.ToLower()
                                       && d.MaVaiTro != maVaiTro);

            if (existingVaiTro != null)
            {
                ModelState.AddModelError("tenVaiTro", "Tên vai trò này đã tồn tại, không được sửa.");
            }

            if (!ModelState.IsValid)
            {
                vt.TenVaiTro = tenVaiTro; // Gán tạm để hiển thị lại trên form
                return View(vt);
            }

            vt.TenVaiTro = tenVaiTro;
            db.Update(vt);
            await db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Sửa vai trò thành công!";
            return RedirectToAction("Index");
        }
    }
}

