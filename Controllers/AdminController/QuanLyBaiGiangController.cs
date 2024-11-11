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
    public class QuanLyBaiGiangController : Controller
    {
        private readonly BaiGiang2024Context _context;

        public QuanLyBaiGiangController(BaiGiang2024Context context)
        {
            _context = context;
        }

        // GET: QuanLyBaiGiang
        public async Task<IActionResult> Index()
        {
            var baiGiang2024Context = _context.Lectures.Include(l => l.Course);
            return View(await baiGiang2024Context.ToListAsync());
        }
        private bool LectureExists(int id)
        {
            return _context.Lectures.Any(e => e.LectureId == id);
        }
    }
}
