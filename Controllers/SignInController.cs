using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BTL.Data;

namespace BTL.Controllers
{
    public class SignInController : Controller
    {
        private readonly WebNcContext _context;

        public SignInController(WebNcContext context)
        {
            _context = context;
        }

        // GET: SignIn
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Register(User model, string fullName, string email, string passwordHash, string confirmPassword)
        {
            if (ModelState.IsValid)
            {

                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email);

                if (existingUser != null)
                {
                    return Json(new { success = false, message = "Email đã tồn tại." });
                }

                var nameParts = fullName.Split(' ');
                var userName = string.Join(' ', nameParts.Skip(1));

                var user = new User
                {
                    FullName = fullName,
                    Email = email,
                    PasswordHash = passwordHash,
                    Username = userName,
                    Role = "Student",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Đăng ký thành công." });
            }
            return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
