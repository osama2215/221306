using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment.Models
{
    public class Students
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? Name { get; set; }
        public string? Major { get; set; }
        public int Mentorid { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
