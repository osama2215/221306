using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment.Data;
using Assignment.Models;

namespace Assignment.Controllers
{
    public class MajorsController : Controller
    {
        private readonly AssignmentContext _context;

        public MajorsController(AssignmentContext context)
        {
            _context = context ?? throw new System.ArgumentNullException(nameof(context));
        }

        // GET: Majors
        public async Task<IActionResult> Index()
        {
            var majors = await _context.Majors
                .AsNoTracking()
                .Where(m => m.IsActive)
                .ToListAsync();

            return View(majors);
        }

        // GET: Majors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var major = await _context.Majors
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.MajorId == id);

            if (major == null) return NotFound();

            return View(major);
        }

        // GET: Majors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Majors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,IsActive")] Major major)
        {
            if (!ModelState.IsValid)
            {
                return View(major);
            }

            try
            {
                _context.Add(major);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Major created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                // surface inner exception message to help debugging in development
                var msg = ex.InnerException?.Message ?? ex.Message;
                ModelState.AddModelError(string.Empty, $"Unable to save changes: {msg}");
                return View(major);
            }
        }

        // GET: Majors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var major = await _context.Majors.FindAsync(id);
            if (major == null) return NotFound();

            return View(major);
        }

        // POST: Majors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MajorId,Name,Description,IsActive")] Major major)
        {
            if (id != major.MajorId) return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(major);
            }

            try
            {
                _context.Update(major);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Major updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MajorExists(major.MajorId)) return NotFound();
                throw;
            }
            catch (DbUpdateException ex)
            {
                var msg = ex.InnerException?.Message ?? ex.Message;
                ModelState.AddModelError(string.Empty, $"Unable to save changes: {msg}");
                return View(major);
            }
        }

        // GET: Majors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var major = await _context.Majors
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.MajorId == id);

            if (major == null) return NotFound();

            return View(major);
        }

        // POST: Majors/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var major = await _context.Majors.FindAsync(id);
            if (major != null)
            {
                _context.Majors.Remove(major);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Major deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool MajorExists(int id)
        {
            return _context.Majors.Any(e => e.MajorId == id);
        }
    }
}
