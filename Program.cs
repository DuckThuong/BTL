using BTL.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình các dịch vụ
builder.Services.AddControllersWithViews();

// Cấu hình DbContext sử dụng SQL Server
builder.Services.AddDbContext<BaiGiang2024Context>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BTL"));
});

// Cấu hình Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<BaiGiang2024Context>()
    .AddDefaultTokenProviders();

// Cấu hình session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login/Login"; 
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login/Login";
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/Login/Login";
    options.LogoutPath = "/Login/Logout";
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
