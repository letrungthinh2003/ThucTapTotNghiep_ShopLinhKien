using LinhKienShop.Models;
using LinhKienShop.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation(); // Hỗ trợ runtime compilation cho Razor

// Cấu hình DbContext
builder.Services.AddDbContext<ShopLinhKienContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Cấu hình Authentication với Cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Đường dẫn đăng nhập
        options.LogoutPath = "/Account/Logout"; // Đường dẫn đăng xuất
        options.AccessDeniedPath = "/Home/AccessDenied"; // Từ chối quyền truy cập
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Thời gian hết hạn cookie
        options.SlidingExpiration = true; // Gia hạn thời gian khi người dùng hoạt động
        options.Cookie.HttpOnly = true; // Ngăn truy cập cookie từ JavaScript
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Chỉ dùng với HTTPS
        options.Cookie.SameSite = SameSiteMode.Strict; // Ngăn CSRF
    });

// Cấu hình Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Cấu hình dịch vụ gửi email
builder.Services.AddScoped<IEmailService, EmailService>();

// Thêm HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Cấu hình logging (tùy chọn chi tiết hơn)
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

// Thêm middleware Authentication và Authorization
app.UseAuthentication(); // Phải trước UseAuthorization
app.UseAuthorization();

// Ngăn cache cho các trang nhạy cảm
app.Use(async (context, next) =>
{
    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    context.Response.Headers["Pragma"] = "no-cache";
    context.Response.Headers["Expires"] = "0";
    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TrangChu}/{action=Index}/{id?}");

app.Run();