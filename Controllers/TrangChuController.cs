using Microsoft.AspNetCore.Mvc;
using BTL.Data;
using Microsoft.EntityFrameworkCore;

namespace BTL.Controllers
{
    public class TrangChuController : Controller
    {
        private readonly BaiGiang2024Context _context;

        public TrangChuController(BaiGiang2024Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> _TrangChu(User Model)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (!string.IsNullOrEmpty(userRole))
            {
                ViewBag.UserRole = userRole;

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
    }
}
