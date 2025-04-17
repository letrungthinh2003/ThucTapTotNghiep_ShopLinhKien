using LinhKienShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace LinhKienShop.Controllers
{
    public class GioHangController : Controller
    {
        private readonly ShopLinhKienContext _context;

        public GioHangController(ShopLinhKienContext context)
        {
            _context = context;
        }

        // Lấy giỏ hàng từ Session
        private List<CartItem> GetCart()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            return string.IsNullOrEmpty(cartJson) ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(cartJson);
        }

        // Lưu giỏ hàng vào Session
        private void SaveCart(List<CartItem> cart)
        {
            var cartJson = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("Cart", cartJson);
        }

        // Thêm sản phẩm vào giỏ hàng từ trang chi tiết sản phẩm
        [HttpPost]
        [Authorize]
        public IActionResult ThemVaoGio(int maSanPham, int soLuong)
        {
            if (soLuong <= 0)
            {
                return Json(new { success = false, message = "Số lượng phải lớn hơn 0." });
            }

            var sanPham = _context.SanPhams.FirstOrDefault(s => s.MaSanPham == maSanPham);
            if (sanPham == null)
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại." });
            }

            if (sanPham.SoLuong == null || sanPham.SoLuong <= 0)
            {
                return Json(new { success = false, message = "Sản phẩm hiện không có sẵn trong kho." });
            }

            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(c => c.MaSanPham == maSanPham);

            if (cartItem != null)
            {
                if (cartItem.SoLuong + soLuong > sanPham.SoLuong)
                {
                    return Json(new { success = false, message = $"Số lượng vượt quá tồn kho. Chỉ còn {sanPham.SoLuong} sản phẩm." });
                }
                cartItem.SoLuong += soLuong;
            }
            else
            {
                if (soLuong > sanPham.SoLuong)
                {
                    return Json(new { success = false, message = $"Số lượng vượt quá tồn kho. Chỉ còn {sanPham.SoLuong} sản phẩm." });
                }
                cart.Add(new CartItem
                {
                    MaSanPham = sanPham.MaSanPham,
                    TenSanPham = sanPham.TenSanPham,
                    HinhSanPhamPath = sanPham.HinhSanPhamPath,
                    GiaKhuyenMai = sanPham.GiaKhuyenMai,
                    SoLuong = soLuong,
                    SoLuongTonKho = sanPham.SoLuong
                });
            }

            SaveCart(cart);
            return Json(new
            {
                success = true,
                message = "Đã thêm sản phẩm vào giỏ hàng thành công!",
                cartCount = cart.Count,
                redirectUrl = Url.Action("Index", "GioHang")
            });
        }

        // Hiển thị trang giỏ hàng
        [Authorize]
        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        // Cập nhật số lượng sản phẩm trong giỏ hàng
        [HttpPost]
        [Authorize]
        public IActionResult CapNhatSoLuong(int maSanPham, int soLuong)
        {
            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(c => c.MaSanPham == maSanPham);

            if (cartItem == null)
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại trong giỏ hàng." });
            }

            if (cartItem.SoLuongTonKho == null || cartItem.SoLuongTonKho <= 0)
            {
                return Json(new { success = false, message = "Sản phẩm hiện không có sẵn trong kho." });
            }

            if (soLuong <= 0)
            {
                cart.Remove(cartItem);
                SaveCart(cart);
                return Json(new
                {
                    success = true,
                    message = "Đã xóa sản phẩm khỏi giỏ hàng.",
                    total = cart.Sum(c => c.ThanhTien),
                    cartCount = cart.Count
                });
            }

            if (soLuong > cartItem.SoLuongTonKho)
            {
                return Json(new { success = false, message = $"Số lượng vượt quá tồn kho. Chỉ còn {cartItem.SoLuongTonKho} sản phẩm." });
            }

            cartItem.SoLuong = soLuong;
            SaveCart(cart);
            return Json(new
            {
                success = true,
                thanhTien = cartItem.ThanhTien,
                total = cart.Sum(c => c.ThanhTien),
                cartCount = cart.Count
            });
        }
        // Xóa sản phẩm khỏi giỏ hàng
        [HttpPost]
        [Authorize]
        public IActionResult XoaKhoiGio(int maSanPham)
        {
            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(c => c.MaSanPham == maSanPham);

            if (cartItem != null)
            {
                cart.Remove(cartItem);
                SaveCart(cart);
                return Json(new
                {
                    success = true,
                    message = "Đã xóa sản phẩm khỏi giỏ hàng.",
                    total = cart.Sum(c => c.ThanhTien), // Không cần ToString("N0") ở đây
                    cartCount = cart.Count
                });
            }

            return Json(new { success = false, message = "Sản phẩm không tồn tại trong giỏ hàng." });
        }
    }
}