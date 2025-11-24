using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assignment.Data;
using Assignment.Models;

namespace Assignment.Controllers
{
    public class CoursesController : Controller
    {
        private readonly AssignmentContext _context;

        public CoursesController(AssignmentContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET: Courses
        public async Task<IActionResult> Index(string? searchString)
        {
            if (_context  == null || _context.Courses == null)
            {
                return Problem("Entity set 'AssignmentContext.Courses' is null.");
            }

            var courses = from s in _context.Courses
                          select s;

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                courses = courses.Where(c => c.Title != null && c.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }

            ViewBag.SearchString = searchString;
            return View(await courses.ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context == null || _context.Courses == null)
            {
                return NotFound();
            }

            var courses = await _context.Courses
                .FirstOrDefaultAsync(m => m.Title == id);
            if (courses == null)
            {
                return NotFound();
            }

            return View(courses);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Credits,Date,CourseType")] Courses courses)
        {
            if (_context == null)
            {
                return Problem("Entity set 'AssignmentContext' is null.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(courses);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(courses);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context == null || _context.Courses == null)
            {
                return NotFound();
            }

            var courses = await _context.Courses.FindAsync(id);
            if (courses == null)
            {
                return NotFound();
            }
            return View(courses);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Title,Description,Credits,Date,CourseType")] Courses courses)
        {
            if (_context == null)
            {
                return Problem("Entity set 'AssignmentContext' is null.");
            }

            if (id != courses.Title) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courses);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoursesExists(courses.Title)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(courses);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context == null || _context.Courses == null)
            {
                return NotFound();
            }

            var courses = await _context.Courses
                .FirstOrDefaultAsync(m => m.Title == id);
            if (courses == null)
            {
                return NotFound();
            }

            return View(courses);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context == null || _context.Courses == null)
            {
                return Problem("Entity set 'AssignmentContext.Courses' is null.");
            }

            var courses = await _context.Courses.FindAsync(id);
            if (courses != null)
            {
                _context.Courses.Remove(courses);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoursesExists(string id)
        {
            return _context != null && _context.Courses != null && _context.Courses.Any(e => e.Title == id);
        }
    }
}
