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
    public class StudentsController : Controller
    {
        private readonly AssignmentContext _context;

        public StudentsController(AssignmentContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string searchString)
        {
            if (_context.Students == null)
            {
                return Problem("Entity set 'AssignmentContext.Student' is null.");
            }
            var students = from s in _context.Students
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.Name!.Contains(searchString));
            }
            return View(await students.ToListAsync());
        }
        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var students = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (students == null)
            {
                return NotFound();
            }

            return View(students);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Major,Mentorid,Address,PhoneNumber")] Students students)
        {
            if (!ModelState.IsValid)
                return View(students);

            _context.Add(students);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var students = await _context.Students.FindAsync(id);
            if (students == null)
            {
                return NotFound();
            }
            return View(students);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int originalId, [Bind("Id,Name,Major,Mentorid,Address,PhoneNumber")] Students students)
        {
            // If user changed the primary key value (Id), handle it explicitly.
            if (originalId != students.Id)
            {
                // Prevent duplicate keys
                if (_context.Students.Any(e => e.Id == students.Id))
                {
                    ModelState.AddModelError("Id", "A student with this Id already exists.");
                    return View(students);
                }

                var existing = await _context.Students.FindAsync(originalId);
                if (existing == null)
                {
                    return NotFound();
                }

                // For SQL Server identity columns: allow explicit insert within a transaction
                // NOTE: this requires appropriate DB permissions and that the table name is correct.
                // If Id is not an identity/auto-generated column, this block still works.
                try
                {
                    await _context.Database.OpenConnectionAsync();
                    using var transaction = await _context.Database.BeginTransactionAsync();

                    // Enable IDENTITY_INSERT for the table so we can insert explicit Id value.
                    // If your table is in a different schema replace dbo.Students accordingly.
                    await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Students ON");

                    _context.Students.Remove(existing);
                    _context.Students.Add(students);
                    await _context.SaveChangesAsync();

                    await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Students OFF");
                    await transaction.CommitAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to update Id — check database constraints and foreign keys.");
                    return View(students);
                }
                finally
                {
                    await _context.Database.CloseConnectionAsync();
                }
            }

            // No Id change: normal update
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(students);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentsExists(students.Id))
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
            return View(students);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var students = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (students == null)
            {
                return NotFound();
            }

            return View(students);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var students = await _context.Students.FindAsync(id);
            if (students != null)
            {
                _context.Students.Remove(students);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentsExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}


