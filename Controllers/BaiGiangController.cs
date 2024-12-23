﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BTL.Data;
using Microsoft.AspNetCore.Authorization;

namespace BTL.Controllers
{   
    [Authorize]
    public class BaiGiangController : Controller
    {
        private readonly BaiGiang2024Context _context;

        public BaiGiangController(BaiGiang2024Context context)
        {
            _context = context;
        }

        // GET: BaiGiang
        public async Task<IActionResult> Index(string query, int page = 1, int pageSize = 9)
        {
            var lecturesQuery = _context.Lectures.Include(l => l.Course).AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                lecturesQuery = lecturesQuery.Where(l => l.LectureName.Contains(query) || l.Title.Contains(query));
            }

            var lectures = await lecturesQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = await lecturesQuery.CountAsync();

            return View(lectures);
        }

        // GET: BaiGiang/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecture = await _context.Lectures
                .Include(l => l.LectureReviews)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(m => m.LectureId == id);
            if (lecture == null)
            {
                return NotFound();
            }

            return View(lecture);
        }

        // GET: BaiGiang/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BaiGiang/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LectureId,LectureName,Image,Title,Content,VideoUrl,Rating,Views,CreatedAt,UpdatedAt")] Lecture lecture)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lecture);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lecture);
        }

        // GET: BaiGiang/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecture = await _context.Lectures.FindAsync(id);
            if (lecture == null)
            {
                return NotFound();
            }
            return View(lecture);
        }

        // POST: BaiGiang/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LectureId,LectureName,Image,Title,Content,VideoUrl,Rating,Views,CreatedAt,UpdatedAt")] Lecture lecture)
        {
            if (id != lecture.LectureId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lecture);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LectureExists(lecture.LectureId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(lecture);
        }

        // GET: BaiGiang/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecture = await _context.Lectures
                .FirstOrDefaultAsync(m => m.LectureId == id);
            if (lecture == null)
            {
                return NotFound();
            }

            return View(lecture);
        }

        // POST: BaiGiang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lecture = await _context.Lectures.FindAsync(id);
            if (lecture != null)
            {
                _context.Lectures.Remove(lecture);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LectureExists(int id)
        {
            return _context.Lectures.Any(e => e.LectureId == id);
        }

        [HttpPost]
        public async Task<IActionResult> Rate([FromBody] RatingRequest request)
        {
            var lecture = await _context.Lectures.FindAsync(request.Id);
            if (lecture == null)
            {
                return Json(new { success = false, message = "Lecture not found" });
            }

            lecture.ReviewCount = (lecture.ReviewCount ?? 0) + 1;
            lecture.Rating = ((lecture.Rating ?? 0) * (lecture.ReviewCount - 1) + request.NewRating) / lecture.ReviewCount;
            var userIdString = HttpContext.Session.GetString("UserId");
            if (userIdString == null)
            {
                return Json(new { success = false, message = "User not logged in" });
            }

            var userId = int.Parse(userIdString);

            var review = new LectureReview
            {
                LectureId = request.Id,
                UserId = userId,
                Rating = request.NewRating,
                Comment = request.ReviewContent,
                CreatedAt = DateTime.Parse(request.Timestamp),
                UpdatedAt = DateTime.Parse(request.Timestamp)
            };

            _context.LectureReviews.Add(review);
            _context.Update(lecture);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        public class RatingRequest
        {
            public int Id { get; set; }
            public int NewRating { get; set; }
            public string ReviewContent { get; set; }
            public string Timestamp { get; set; }
        }
    }
}
