using LinhKienShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LinhKienShop.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ShopLinhKienContext db;

        public DashboardController(ShopLinhKienContext context)
        {
            db = context;
        }

        // GET: Dashboard
        public async Task<IActionResult> Index()
        {
            // Lấy tổng số sản phẩm từ cơ sở dữ liệu
            int totalProducts = await db.SanPhams.CountAsync();
            ViewBag.TotalProducts = totalProducts;

            // Có thể thêm các số liệu thống kê khác (ví dụ: tổng khách hàng)
            int totalCustomers = 150; // Giả lập, thay bằng truy vấn thực tế nếu có bảng khách hàng
            ViewBag.TotalCustomers = totalCustomers;

            ViewData["Title"] = "Dashboard";
            return View();
        }
    }
}