using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BTL.Data;
using Microsoft.AspNetCore.Identity;

namespace BTL.Controllers
{
    public class LoginController : Controller
    {
        private readonly WebNcContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public LoginController(WebNcContext context, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User model)
        {
            if (!ModelState.IsValid)
                return View("Login");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Không tìm thấy tài khoản");
                return View("Login");
            }

            if (user.PasswordHash != model.PasswordHash) 
            {
                ModelState.AddModelError(string.Empty, "Không thể đăng nhập");
                return View("Login");
            }
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserPasswordHash", user.PasswordHash);

            return RedirectToAction("Index", "Home"); // Redirect to Home/Index on success
        }
    }
}
