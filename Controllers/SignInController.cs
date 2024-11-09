using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BTL.Data;
using System.Text;
using System.Security.Cryptography;

namespace BTL.Controllers
{
    public class SignInController : Controller
    {
        private readonly BaiGiang2024Context _context;

        public SignInController(BaiGiang2024Context context)
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
            if (passwordHash != confirmPassword)
            {
                return Json(new { success = false, message = "Mật khẩu không khớp.", errorCode = "PASSWORD_MISMATCH" });
            }

            if (passwordHash.Length < 8)
            {
                return Json(new { success = false, message = "Mật khẩu phải có ít nhất 8 ký tự.", errorCode = "PASSWORD_TOO_SHORT" });
            }

            if (!passwordHash.Any(char.IsDigit))
            {
                return Json(new { success = false, message = "Mật khẩu phải chứa ít nhất một chữ số.", errorCode = "PASSWORD_NO_DIGIT" });
            }

            if (!passwordHash.Any(char.IsUpper))
            {
                return Json(new { success = false, message = "Mật khẩu phải chứa ít nhất một chữ hoa.", errorCode = "PASSWORD_NO_UPPERCASE" });
            }

            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ.", errorCode = "INVALID_DATA" });
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (existingUser != null)
            {
                return Json(new { success = false, message = "Email đã tồn tại.", errorCode = "USER_EXISTS" });
            }

            try
            {
                var user = new User
                {
                    FullName = fullName,
                    Email = email,
                    PasswordHash = passwordHash,
                    Username = fullName,
                    Role = "Student",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Đăng ký thành công.", userId = user.UserId });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Đăng kí thất bại", error = ex.Message });
            }
        }


        public static string GetMD5(string str)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] fromData = Encoding.UTF8.GetBytes(str);
                byte[] targetData = md5.ComputeHash(fromData);
                StringBuilder byte2String = new StringBuilder();

                for (int i = 0; i < targetData.Length; i++)
                {
                    byte2String.Append(targetData[i].ToString("x2"));
                }
                return byte2String.ToString();
            }
        }
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
