using System;
using Microsoft.EntityFrameworkCore;
using Assignment.Models;

namespace Assignment.Data
{
    public class AssignmentContext : DbContext
    {
        public AssignmentContext (DbContextOptions<AssignmentContext> options)
            : base(options)
        {
        }

        public DbSet<Assignment.Models.Students> Students { get; set; } = default!;
        public DbSet<Courses> Courses { get; set; } = default!;
        public DbSet<Major> Majors { get; set; } = default!;
    }
}
