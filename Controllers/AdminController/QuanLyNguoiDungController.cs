using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BTL.Data;

namespace BTL.Controllers.AdminController
{
    public class QuanLyNguoiDungController : Controller
    {
        private readonly BaiGiang2024Context _context;

        public QuanLyNguoiDungController(BaiGiang2024Context context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return Json(new { success = false, message = "Có vấn đề xảy ra" });
            }

            var relatedReviews = _context.LectureReviews.Where(r => r.UserId == id);
            _context.LectureReviews.RemoveRange(relatedReviews);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Xóa người dùng thành công";
            return Json(new { success = true, message = "Xóa thành công" });
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
