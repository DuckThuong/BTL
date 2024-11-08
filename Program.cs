using BTL.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình các dịch vụ
builder.Services.AddControllersWithViews();

// Cấu hình DbContext sử dụng SQL Server
builder.Services.AddDbContext<WebNcContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BTL"));
});

// Cấu hình Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<WebNcContext>()
    .AddDefaultTokenProviders();

// Cấu hình session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // thời gian timeout cho session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Cấu hình middleware

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Kích hoạt session
app.UseSession();

// Kích hoạt xác thực và phân quyền
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "trangchu",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
