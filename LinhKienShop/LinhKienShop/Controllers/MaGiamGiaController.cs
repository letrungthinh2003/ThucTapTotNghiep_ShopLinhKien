using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LinhKienShop.Models;
using System.Linq;
using System.Threading.Tasks;

namespace LinhKienShop.Controllers
{
    public class MaGiamGiaController : Controller
    {
        private readonly ShopLinhKienContext _context;

        public MaGiamGiaController(ShopLinhKienContext context)
        {
            _context = context;
        }

        // GET: Danh sách mã giảm giá
        public async Task<IActionResult> Index()
        {
            var maGiamGias = await _context.MaGiamGia
                .Include(m => m.MaLoaiGiamGiaNavigation)
                .Select(m => new MaGiamGiaViewModel
                {
                    MaGiamGiaId = m.MaGiamGiaId,
                    MaCode = m.MaCode,
                    MaLoaiGiamGia = m.MaLoaiGiamGia,
                    GiaTriGiam = m.GiaTriGiam,
                    GiaTriGiamToiDa = m.GiaTriGiamToiDa,
                    DonHangToiThieu = m.DonHangToiThieu,
                    NgayBatDau = m.NgayBatDau,
                    NgayHetHan = m.NgayHetHan,
                    TrangThai = m.TrangThai
                })
                .ToListAsync();

            ViewBag.LoaiGiamGias = await _context.LoaiGiamGia.ToListAsync();
            return View(maGiamGias);
        }

        // POST: Bật/tắt trạng thái
        [HttpPost]
        public async Task<IActionResult> ToggleTrangThai(int id)
        {
            var maGiamGia = await _context.MaGiamGia.FindAsync(id);
            if (maGiamGia == null)
            {
                return Json(new { success = false, message = "Mã giảm giá không tồn tại." });
            }

            maGiamGia.TrangThai = !maGiamGia.TrangThai;
            _context.Update(maGiamGia);
            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                trangThai = maGiamGia.TrangThai,
                trangThaiText = maGiamGia.TrangThai ? "Hoạt động" : "Bị khóa"
            });
        }

        // GET: Thêm mã giảm giá
        public IActionResult Them()
        {
            ViewBag.LoaiGiamGias = _context.LoaiGiamGia.ToList();
            return View(new MaGiamGiaViewModel());
        }

        // POST: Thêm mã giảm giá
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Them(MaGiamGiaViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.NgayHetHan < model.NgayBatDau)
                {
                    ModelState.AddModelError("NgayHetHan", "Ngày hết hạn phải lớn hơn hoặc bằng ngày bắt đầu.");
                    ViewBag.LoaiGiamGias = _context.LoaiGiamGia.ToList();
                    return View(model);
                }

                if (await _context.MaGiamGia.AnyAsync(m => m.MaCode == model.MaCode))
                {
                    ModelState.AddModelError("MaCode", "Mã code đã tồn tại.");
                    ViewBag.LoaiGiamGias = _context.LoaiGiamGia.ToList();
                    return View(model);
                }

                var maGiamGia = new MaGiamGium
                {
                    MaCode = model.MaCode,
                    MaLoaiGiamGia = model.MaLoaiGiamGia,
                    GiaTriGiam = model.GiaTriGiam,
                    GiaTriGiamToiDa = model.GiaTriGiamToiDa,
                    DonHangToiThieu = model.DonHangToiThieu,
                    NgayBatDau = model.NgayBatDau,
                    NgayHetHan = model.NgayHetHan,
                    TrangThai = model.TrangThai
                };

                _context.Add(maGiamGia);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm mã giảm giá thành công!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.LoaiGiamGias = _context.LoaiGiamGia.ToList();
            return View(model);
        }

        // GET: Sửa mã giảm giá
        public async Task<IActionResult> Sua(int id)
        {
            var maGiamGia = await _context.MaGiamGia.FindAsync(id);
            if (maGiamGia == null)
            {
                TempData["ErrorMessage"] = "Mã giảm giá không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            var model = new MaGiamGiaViewModel
            {
                MaGiamGiaId = maGiamGia.MaGiamGiaId,
                MaCode = maGiamGia.MaCode,
                MaLoaiGiamGia = maGiamGia.MaLoaiGiamGia,
                GiaTriGiam = maGiamGia.GiaTriGiam,
                GiaTriGiamToiDa = maGiamGia.GiaTriGiamToiDa,
                DonHangToiThieu = maGiamGia.DonHangToiThieu,
                NgayBatDau = maGiamGia.NgayBatDau,
                NgayHetHan = maGiamGia.NgayHetHan,
                TrangThai = maGiamGia.TrangThai
            };

            ViewBag.LoaiGiamGias = _context.LoaiGiamGia.ToList();
            return View(model);
        }

        // POST: Sửa mã giảm giá
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sua(int id, MaGiamGiaViewModel model)
        {
            if (id != model.MaGiamGiaId)
            {
                TempData["ErrorMessage"] = "ID không hợp lệ.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                if (model.NgayHetHan < model.NgayBatDau)
                {
                    ModelState.AddModelError("NgayHetHan", "Ngày hết hạn phải lớn hơn hoặc bằng ngày bắt đầu.");
                    ViewBag.LoaiGiamGias = _context.LoaiGiamGia.ToList();
                    return View(model);
                }

                if (await _context.MaGiamGia.AnyAsync(m => m.MaCode == model.MaCode && m.MaGiamGiaId != id))
                {
                    ModelState.AddModelError("MaCode", "Mã code đã tồn tại.");
                    ViewBag.LoaiGiamGias = _context.LoaiGiamGia.ToList();
                    return View(model);
                }

                var maGiamGia = await _context.MaGiamGia.FindAsync(id);
                if (maGiamGia == null)
                {
                    TempData["ErrorMessage"] = "Mã giảm giá không tồn tại.";
                    return RedirectToAction(nameof(Index));
                }

                maGiamGia.MaCode = model.MaCode;
                maGiamGia.MaLoaiGiamGia = model.MaLoaiGiamGia;
                maGiamGia.GiaTriGiam = model.GiaTriGiam;
                maGiamGia.GiaTriGiamToiDa = model.GiaTriGiamToiDa;
                maGiamGia.DonHangToiThieu = model.DonHangToiThieu;
                maGiamGia.NgayBatDau = model.NgayBatDau;
                maGiamGia.NgayHetHan = model.NgayHetHan;
                maGiamGia.TrangThai = model.TrangThai;

                _context.Update(maGiamGia);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Sửa mã giảm giá thành công!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.LoaiGiamGias = _context.LoaiGiamGia.ToList();
            return View(model);
        }

        // GET: Xóa mã giảm giá
        public async Task<IActionResult> Xoa(int id)
        {
            var maGiamGia = await _context.MaGiamGia
                .Include(m => m.MaLoaiGiamGiaNavigation)
                .FirstOrDefaultAsync(m => m.MaGiamGiaId == id);

            if (maGiamGia == null)
            {
                TempData["ErrorMessage"] = "Mã giảm giá không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            var model = new MaGiamGiaViewModel
            {
                MaGiamGiaId = maGiamGia.MaGiamGiaId,
                MaCode = maGiamGia.MaCode,
                MaLoaiGiamGia = maGiamGia.MaLoaiGiamGia,
                GiaTriGiam = maGiamGia.GiaTriGiam,
                GiaTriGiamToiDa = maGiamGia.GiaTriGiamToiDa,
                DonHangToiThieu = maGiamGia.DonHangToiThieu,
                NgayBatDau = maGiamGia.NgayBatDau,
                NgayHetHan = maGiamGia.NgayHetHan,
                TrangThai = maGiamGia.TrangThai
            };

            return View(model);
        }

        // POST: Xóa mã giảm giá
        [HttpPost, ActionName("Xoa")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaConfirmed(int id)
        {
            var maGiamGia = await _context.MaGiamGia.FindAsync(id);
            if (maGiamGia == null)
            {
                TempData["ErrorMessage"] = "Mã giảm giá không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            _context.MaGiamGia.Remove(maGiamGia);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Xóa mã giảm giá thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}