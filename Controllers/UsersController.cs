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
    public class UsersController : Controller
    {
        private readonly BaiGiang2024Context _context;

        public UsersController(BaiGiang2024Context context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (userIdString == null)
            {
                return Json(new { success = false, message = "User not logged in" });
            }

            var userId = int.Parse(userIdString);
            if (userId == null)
            {
                return NotFound("User ID not found in session.");
            }

            // Fetch the user based on the retrieved ID
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(int id, User user)
        {
            try
            {
                var userIdString = HttpContext.Session.GetString("UserId");
                if (userIdString == null)
                {
                    return Json(new { success = false, message = "User not logged in" });
                }

                var userId = int.Parse(userIdString);
                var existingUser = await _context.Users.FindAsync(userId);
                if (existingUser == null)
                {
                    return NotFound();
                }

                // Update the properties of the existing user
                existingUser.Username = user.Username;
                existingUser.Email = user.Email;
                existingUser.FullName = user.FullName;
                existingUser.Gender = user.Gender;
                existingUser.BirthYear = user.BirthYear;
                existingUser.Age = user.Age;
                existingUser.Address = user.Address;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.ProfilePicture = user.ProfilePicture;
                existingUser.Status = user.Status;
                existingUser.Nationality = user.Nationality;
                existingUser.Occupation = user.Occupation;
                existingUser.Bio = user.Bio;
                existingUser.Interests = user.Interests;
                existingUser.Role = user.Role;
                existingUser.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.UserId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
