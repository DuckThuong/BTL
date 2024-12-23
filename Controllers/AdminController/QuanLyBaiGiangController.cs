﻿using System;
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

        [HttpPut]
        public async Task<IActionResult> Update(int id,[FromBody] Lecture lecture)
        {
            if (!LectureExists(id))
            {
                return Json(new { success = false });
            }

            try
            {
                var existingLecture = await _context.Lectures.FindAsync(id);
                if (existingLecture != null)
                {
                    existingLecture.LectureName = lecture.LectureName;
                    existingLecture.CourseInfo = lecture.CourseInfo;
                    existingLecture.Title = lecture.Title;
                    existingLecture.Image = lecture.Image;
                    existingLecture.Content = lecture.Content;
                    existingLecture.VideoUrl = lecture.VideoUrl;
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            catch (DbUpdateConcurrencyException)
            {
                return Json(new { success = false });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var lecture = await _context.Lectures.FindAsync(id);
            if (lecture == null)
            {
                return Json(new { success = false, message = "Bài giảng không tìm thấy." });
            }

            try
            {
                var relatedReviews = _context.LectureReviews.Where(r => r.LectureId == id);
                _context.LectureReviews.RemoveRange(relatedReviews);

                _context.Lectures.Remove(lecture);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Đã xảy ra lỗi khi xóa bài giảng: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Lecture lecture)
        {
            if (ModelState.IsValid)
            {
                if (!_context.Courses.Any(c => c.CourseId == lecture.CourseId))
                {
                    return Json(new { success = false, message = "CourseId không hợp lệ. Khóa học được chỉ định không tồn tại." });
                }

                try
                {
                    _context.Lectures.Add(lecture);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Đã xảy ra lỗi khi thêm bài giảng: " + ex.Message);
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine("Ngoại lệ bên trong: " + ex.InnerException.Message);
                    }
                    return Json(new { success = false, message = "Đã xảy ra lỗi khi thêm bài giảng: " + ex.Message });
                }
            }
            return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
        }

        [HttpGet]
        public async Task<IActionResult> ShowModalData(int id)
        {
            var lecture = await _context.Lectures.FindAsync(id);
            if (lecture == null)
            {
                return Json(new { success = false, message = "Bài giảng không tìm thấy." });
            }

            return Json(new
            {
                success = true,
                data = new
                {
                    lecture.LectureName,
                    lecture.CourseInfo,
                    lecture.Image,
                    lecture.Title,
                    lecture.Content,
                    lecture.VideoUrl,
                }
            });
        }

        private bool LectureExists(int id)
        {
            return _context.Lectures.Any(e => e.LectureId == id);
        }
    }
}
