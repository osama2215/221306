using System.ComponentModel.DataAnnotations;

namespace Assignment.Models
{
    public enum CourseType
    {
        Main = 0,
        Elective = 1
    }

    public class Courses
    {
        [Key]
        public string? Title { get; set; }
        public string? Description { get; set; }
        [Range(3,4)]
        public int Credits { get; set; }
        public DateTime Date { get; set; }

    }
}
