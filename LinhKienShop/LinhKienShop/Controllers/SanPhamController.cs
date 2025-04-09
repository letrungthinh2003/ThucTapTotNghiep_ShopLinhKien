using LinhKienShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LinhKienShop.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly ShopLinhKienContext _context;

        public SanPhamController(ShopLinhKienContext context)
        {
            _context = context;
        }

        // GET: Index với phân trang
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 6; // Số sản phẩm mỗi trang
            var sanPhams = _context.SanPhams
                .Include(s => s.MaDanhMucNavigation)
                .Include(s => s.MaThuongHieuNavigation)
                .Include(s => s.MaXuatXuNavigation);

            // Tính tổng số sản phẩm và tổng số trang
            int totalItems = await sanPhams.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Lấy danh sách sản phẩm cho trang hiện tại
            var pagedSanPhams = await sanPhams
                .OrderBy(s => s.MaSanPham) // Sắp xếp theo MaSanPham (có thể thay đổi tiêu chí)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Truyền thông tin phân trang qua ViewBag
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = page > 1;
            ViewBag.HasNext = page < totalPages;

            return View(pagedSanPhams);
        }

        // GET: Thêm
        public IActionResult Them()
        {
            ViewBag.DanhMuc = _context.DanhMucs.ToList();
            ViewBag.ThuongHieu = _context.ThuongHieus.ToList();
            ViewBag.XuatXu = _context.XuatXus.ToList();
            return View(new SanPham());
        }

        // POST: Thêm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Them([Bind("TenSanPham,GiaGoc,GiaKhuyenMai,SoLuong,MaDanhMuc,MaThuongHieu,MaXuatXu,HinhSanPhamUpload,HinhChiTietUploads,ThongSoKyThuat")] SanPham sanPham)
        {
            // Kiểm tra các ràng buộc nghiệp vụ (giữ nguyên như code cũ)
            if (string.IsNullOrEmpty(sanPham.TenSanPham))
            {
                ModelState.AddModelError("TenSanPham", "Tên sản phẩm không được để trống.");
            }
            if (sanPham.GiaGoc <= 0)
            {
                ModelState.AddModelError("GiaGoc", "Giá gốc phải lớn hơn 0.");
            }
            if (sanPham.GiaKhuyenMai < 0 || sanPham.GiaKhuyenMai >= sanPham.GiaGoc)
            {
                ModelState.AddModelError("GiaKhuyenMai", "Giá khuyến mãi phải từ 0 và nhỏ hơn giá gốc.");
            }
            if (sanPham.SoLuong < 0)
            {
                ModelState.AddModelError("SoLuong", "Số lượng phải lớn hơn hoặc bằng 0.");
            }
            if (_context.SanPhams.Any(s => s.TenSanPham == sanPham.TenSanPham))
            {
                ModelState.AddModelError("TenSanPham", "Tên sản phẩm đã tồn tại.");
            }
            if (sanPham.MaDanhMuc <= 0 || !_context.DanhMucs.Any(d => d.MaDanhMuc == sanPham.MaDanhMuc))
            {
                ModelState.AddModelError("MaDanhMuc", "Vui lòng chọn danh mục hợp lệ.");
            }
            if (sanPham.MaThuongHieu <= 0 || !_context.ThuongHieus.Any(t => t.MaThuongHieu == sanPham.MaThuongHieu))
            {
                ModelState.AddModelError("MaThuongHieu", "Vui lòng chọn thương hiệu hợp lệ.");
            }
            if (sanPham.MaXuatXu <= 0 || !_context.XuatXus.Any(x => x.MaXuatXu == sanPham.MaXuatXu))
            {
                ModelState.AddModelError("MaXuatXu", "Vui lòng chọn xuất xứ hợp lệ.");
            }

            // Xử lý hình ảnh chính
            if (sanPham.HinhSanPhamUpload == null)
            {
                ModelState.AddModelError("HinhSanPhamUpload", "Hình ảnh chính là bắt buộc.");
            }
            else
            {
                var fileExtension = Path.GetExtension(sanPham.HinhSanPhamUpload.FileName).ToLower();
                var validExtensions = new[] { ".jpg", ".jpeg", ".png" };
                if (!validExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("HinhSanPhamUpload", "Chỉ chấp nhận file JPG, PNG, JPEG.");
                }
                if (sanPham.HinhSanPhamUpload.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("HinhSanPhamUpload", "Dung lượng file phải nhỏ hơn hoặc bằng 2MB.");
                }
            }

            // Xử lý hình ảnh chi tiết (nếu có)
            if (sanPham.HinhChiTietUploads != null && sanPham.HinhChiTietUploads.Any())
            {
                foreach (var file in sanPham.HinhChiTietUploads)
                {
                    if (file != null)
                    {
                        var fileExtension = Path.GetExtension(file.FileName).ToLower();
                        var validExtensions = new[] { ".jpg", ".jpeg", ".png" };
                        if (!validExtensions.Contains(fileExtension))
                        {
                            ModelState.AddModelError("HinhChiTietUploads", "Hình chi tiết chỉ chấp nhận file JPG, PNG, JPEG.");
                        }
                        if (file.Length > 2 * 1024 * 1024)
                        {
                            ModelState.AddModelError("HinhChiTietUploads", "Dung lượng file hình chi tiết phải nhỏ hơn hoặc bằng 2MB.");
                        }
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.DanhMuc = _context.DanhMucs.ToList();
                ViewBag.ThuongHieu = _context.ThuongHieus.ToList();
                ViewBag.XuatXu = _context.XuatXus.ToList();
                TempData["Error"] = "Có lỗi trong dữ liệu nhập. Vui lòng kiểm tra lại.";
                return View(sanPham);
            }

            // Lưu hình ảnh chính
            var imagesDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            if (!Directory.Exists(imagesDir))
            {
                Directory.CreateDirectory(imagesDir);
            }
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(sanPham.HinhSanPhamUpload.FileName).ToLower();
            var filePath = Path.Combine(imagesDir, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await sanPham.HinhSanPhamUpload.CopyToAsync(stream);
            }
            sanPham.HinhSanPhamPath = "/images/" + fileName;

            // Gán giá trị mặc định
            sanPham.NgayTao = DateTime.Now;
            sanPham.TrangThai = "HoatDong";

            try
            {
                // Thêm sản phẩm vào database trước
                _context.SanPhams.Add(sanPham);
                await _context.SaveChangesAsync();

                // Lưu các hình chi tiết (nếu có)
                if (sanPham.HinhChiTietUploads != null && sanPham.HinhChiTietUploads.Any())
                {
                    foreach (var file in sanPham.HinhChiTietUploads.Where(f => f != null))
                    {
                        var chiTietFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName).ToLower();
                        var chiTietFilePath = Path.Combine(imagesDir, chiTietFileName);
                        using (var stream = new FileStream(chiTietFilePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        var hinhChiTiet = new HinhChiTietSlider
                        {
                            MaSanPham = sanPham.MaSanPham,
                            LinkHinhAnh = "/images/" + chiTietFileName,
                            LoaiHinhAnh = "hinhphu" // Gán mặc định là hình phụ, bạn có thể thay đổi logic nếu cần
                        };
                        _context.HinhChiTietSliders.Add(hinhChiTiet);
                    }
                    await _context.SaveChangesAsync();
                }

                TempData["Success"] = "Thêm sản phẩm thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi thêm sản phẩm: " + ex.Message;
                ViewBag.DanhMuc = _context.DanhMucs.ToList();
                ViewBag.ThuongHieu = _context.ThuongHieus.ToList();
                ViewBag.XuatXu = _context.XuatXus.ToList();
                return View(sanPham);
            }
        }

        // GET: Chi tiết
        public async Task<IActionResult> ChiTiet(int? id)
        {
            if (id == null) return NotFound();

            var sanPham = await _context.SanPhams
                .Include(s => s.MaDanhMucNavigation)
                .Include(s => s.MaThuongHieuNavigation)
                .Include(s => s.MaXuatXuNavigation)
                .Include(s => s.HinhChiTietSliders) // Load danh sách hình ảnh chi tiết
                .FirstOrDefaultAsync(m => m.MaSanPham == id);

            if (sanPham == null) return NotFound();

            return View(sanPham);
        }
        


        // POST: Cập nhật trạng thái
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleTrangThai(int id)
        {
            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham == null) return NotFound();

            sanPham.TrangThai = sanPham.TrangThai == "HoatDong" ? "BiKhoa" : "HoatDong";
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Sửa
        public async Task<IActionResult> Sua(int? id)
        {
            if (id == null) return NotFound();

            var sanPham = await _context.SanPhams
                .Include(s => s.HinhChiTietSliders) // Load danh sách hình ảnh chi tiết
                .FirstOrDefaultAsync(s => s.MaSanPham == id);
            if (sanPham == null) return NotFound();

            ViewBag.DanhMuc = _context.DanhMucs.ToList();
            ViewBag.ThuongHieu = _context.ThuongHieus.ToList();
            ViewBag.XuatXu = _context.XuatXus.ToList();
            return View(sanPham);
        }

        // POST: Sửa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sua(int id, [Bind("MaSanPham,TenSanPham,GiaGoc,GiaKhuyenMai,SoLuong,MaDanhMuc,MaThuongHieu,MaXuatXu,HinhSanPhamUpload,HinhChiTietUploads,ThongSoKyThuat,TrangThai,NgayTao,HinhSanPhamPath")] SanPham sanPham, int[] hinhChiTietIdsToDelete)
        {
            if (id != sanPham.MaSanPham) return NotFound();

            // Kiểm tra ràng buộc nghiệp vụ
            if (string.IsNullOrEmpty(sanPham.TenSanPham))
            {
                ModelState.AddModelError("TenSanPham", "Tên sản phẩm không được để trống.");
            }
            if (sanPham.GiaGoc <= 0)
            {
                ModelState.AddModelError("GiaGoc", "Giá gốc phải lớn hơn 0.");
            }
            if (sanPham.GiaKhuyenMai < 0 || sanPham.GiaKhuyenMai >= sanPham.GiaGoc)
            {
                ModelState.AddModelError("GiaKhuyenMai", "Giá khuyến mãi phải từ 0 và nhỏ hơn giá gốc.");
            }
            if (sanPham.SoLuong < 0)
            {
                ModelState.AddModelError("SoLuong", "Số lượng phải lớn hơn hoặc bằng 0.");
            }
            if (_context.SanPhams.Any(s => s.TenSanPham == sanPham.TenSanPham && s.MaSanPham != sanPham.MaSanPham))
            {
                ModelState.AddModelError("TenSanPham", "Tên sản phẩm đã tồn tại.");
            }
            if (sanPham.MaDanhMuc <= 0 || !_context.DanhMucs.Any(d => d.MaDanhMuc == sanPham.MaDanhMuc))
            {
                ModelState.AddModelError("MaDanhMuc", "Vui lòng chọn danh mục hợp lệ.");
            }
            if (sanPham.MaThuongHieu <= 0 || !_context.ThuongHieus.Any(t => t.MaThuongHieu == sanPham.MaThuongHieu))
            {
                ModelState.AddModelError("MaThuongHieu", "Vui lòng chọn thương hiệu hợp lệ.");
            }
            if (sanPham.MaXuatXu <= 0 || !_context.XuatXus.Any(x => x.MaXuatXu == sanPham.MaXuatXu))
            {
                ModelState.AddModelError("MaXuatXu", "Vui lòng chọn xuất xứ hợp lệ.");
            }

            // Xử lý hình ảnh chính nếu có upload mới
            if (sanPham.HinhSanPhamUpload != null)
            {
                var fileExtension = Path.GetExtension(sanPham.HinhSanPhamUpload.FileName).ToLower();
                var validExtensions = new[] { ".jpg", ".jpeg", ".png" };
                if (!validExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("HinhSanPhamUpload", "Chỉ chấp nhận file JPG, PNG, JPEG.");
                }
                if (sanPham.HinhSanPhamUpload.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("HinhSanPhamUpload", "Dung lượng file phải nhỏ hơn hoặc bằng 2MB.");
                }
            }

            // Xử lý hình ảnh chi tiết nếu có upload mới
            if (sanPham.HinhChiTietUploads != null && sanPham.HinhChiTietUploads.Any())
            {
                foreach (var file in sanPham.HinhChiTietUploads.Where(f => f != null))
                {
                    var fileExtension = Path.GetExtension(file.FileName).ToLower();
                    var validExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    if (!validExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("HinhChiTietUploads", "Hình chi tiết chỉ chấp nhận file JPG, PNG, JPEG.");
                    }
                    if (file.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("HinhChiTietUploads", "Dung lượng file hình chi tiết phải nhỏ hơn hoặc bằng 2MB.");
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.DanhMuc = _context.DanhMucs.ToList();
                ViewBag.ThuongHieu = _context.ThuongHieus.ToList();
                ViewBag.XuatXu = _context.XuatXus.ToList();
                TempData["Error"] = "Có lỗi trong dữ liệu nhập. Vui lòng kiểm tra lại.";
                return View(sanPham);
            }

            var imagesDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            if (!Directory.Exists(imagesDir))
            {
                Directory.CreateDirectory(imagesDir);
            }

            // Xử lý hình ảnh chính nếu có upload mới
            if (sanPham.HinhSanPhamUpload != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(sanPham.HinhSanPhamUpload.FileName).ToLower();
                var filePath = Path.Combine(imagesDir, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await sanPham.HinhSanPhamUpload.CopyToAsync(stream);
                }
                // Xóa hình cũ nếu có
                if (!string.IsNullOrEmpty(sanPham.HinhSanPhamPath))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", sanPham.HinhSanPhamPath.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
                sanPham.HinhSanPhamPath = "/images/" + fileName;
            }

            try
            {
                // Xóa các hình ảnh chi tiết được chọn
                if (hinhChiTietIdsToDelete != null && hinhChiTietIdsToDelete.Any())
                {
                    var hinhChiTietsToDelete = await _context.HinhChiTietSliders
                        .Where(h => hinhChiTietIdsToDelete.Contains(h.MaHinhChiTietSlider))
                        .ToListAsync();
                    foreach (var hinh in hinhChiTietsToDelete)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", hinh.LinkHinhAnh.TrimStart('/'));
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                        _context.HinhChiTietSliders.Remove(hinh);
                    }
                }

                // Thêm các hình ảnh chi tiết mới
                if (sanPham.HinhChiTietUploads != null && sanPham.HinhChiTietUploads.Any())
                {
                    foreach (var file in sanPham.HinhChiTietUploads.Where(f => f != null))
                    {
                        var chiTietFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName).ToLower();
                        var chiTietFilePath = Path.Combine(imagesDir, chiTietFileName);
                        using (var stream = new FileStream(chiTietFilePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        _context.HinhChiTietSliders.Add(new HinhChiTietSlider
                        {
                            MaSanPham = sanPham.MaSanPham,
                            LinkHinhAnh = "/images/" + chiTietFileName,
                            LoaiHinhAnh = "hinhphu"
                        });
                    }
                }

                _context.Update(sanPham);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi cập nhật sản phẩm: " + ex.Message;
                ViewBag.DanhMuc = _context.DanhMucs.ToList();
                ViewBag.ThuongHieu = _context.ThuongHieus.ToList();
                ViewBag.XuatXu = _context.XuatXus.ToList();
                return View(sanPham);
            }
        }

        // GET: Xóa
        public async Task<IActionResult> Xoa(int? id)
        {
            if (id == null) return NotFound();

            var sanPham = await _context.SanPhams
                .Include(s => s.MaDanhMucNavigation)
                .Include(s => s.MaThuongHieuNavigation)
                .Include(s => s.MaXuatXuNavigation)
                .Include(s => s.HinhChiTietSliders) // Load danh sách hình ảnh chi tiết
                .FirstOrDefaultAsync(m => m.MaSanPham == id);

            if (sanPham == null) return NotFound();

            return View(sanPham);
        }

        // POST: Xóa
        [HttpPost, ActionName("Xoa")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaConfirmed(int id)
        {
            var sanPham = await _context.SanPhams
                .Include(s => s.HinhChiTietSliders) // Load danh sách hình ảnh chi tiết
                .FirstOrDefaultAsync(s => s.MaSanPham == id);
            if (sanPham == null) return NotFound();

            var imagesDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            // Xóa hình ảnh chính nếu có
            if (!string.IsNullOrEmpty(sanPham.HinhSanPhamPath))
            {
                var filePath = Path.Combine(imagesDir, sanPham.HinhSanPhamPath.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            // Xóa tất cả hình ảnh chi tiết
            if (sanPham.HinhChiTietSliders.Any())
            {
                foreach (var hinh in sanPham.HinhChiTietSliders)
                {
                    var filePath = Path.Combine(imagesDir, hinh.LinkHinhAnh.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }

            try
            {
                _context.SanPhams.Remove(sanPham); // Xóa sản phẩm sẽ tự động xóa các bản ghi liên quan trong HinhChiTietSlider do ON DELETE CASCADE
                await _context.SaveChangesAsync();
                TempData["Success"] = "Xóa sản phẩm thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi xóa sản phẩm: " + ex.Message;
                return RedirectToAction(nameof(Xoa), new { id });
            }
        }
    }
}