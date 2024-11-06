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
            //if (!ModelState.IsValid)
                //return Json(new { success = false, message = "Dữ liệu không hợp lệ" });

            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                return Json(new { success = false, message = "Không tìm thấy tài khoản" });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.PasswordHash, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Không thể đăng nhập" });
        }
       
    }
}
