using Assignment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Assignment.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider, AssignmentContext context)
        {
            using (var a = new AssignmentContext(
            serviceProvider.GetRequiredService<
            DbContextOptions<AssignmentContext>>()))
            {
                // Look for any students.
                if (context.Students.Any())
                {
                    return; // DB has been seeded
                }

                context.Students.AddRange(
                new Students
                {
                    Name = "Alice Smith",
                    Major = "CS",
                    Mentorid = 1,
                    Address = "123 Maple Street, Springfield, IL",
                    PhoneNumber = "555-1234"
                },
                new Students
                {
                    Name = "Bob Johnson",
                    Major = "CS",
                    Mentorid = 2,
                    Address = "456 Oak Avenue, Metropolis, NY",
                    PhoneNumber = "555-5678"
                },
                new Students
                {
                    Name = "Charlie Brown",
                    Major = "CS",
                    Mentorid = 5,
                    Address = "789 Pine Road, Smalltown, TX",
                    PhoneNumber = "555-8765"
                });

                if (!context.Courses.Any())
                {
                    context.Courses.AddRange(
                        new Courses { Title = "Mathematics", Description = "basic Mathematics", Credits = 3, Date = DateTime.UtcNow },
                        new Courses { Title = "Biology", Description = "introduction in Biology", Credits = 4, Date = DateTime.UtcNow },
                        new Courses { Title = "Engineering", Description = "introduction in Engineering", Credits = 3, Date = DateTime.UtcNow }
                    );
                }

                
            }
        }
    }
}