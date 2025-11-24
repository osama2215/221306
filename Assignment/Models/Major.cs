using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment.Models
{
    public class Major
    {
        [Key]
        [Display(Name = "Major ID")]
        public int MajorId { get; set; }

        [Required(ErrorMessage = "Major name is required")]
        [StringLength(100, ErrorMessage = "Major name cannot exceed 100 characters")]
        [Display(Name = "Major Name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        // Navigation properties
        public virtual ICollection<Students> Students { get; set; } = new List<Students>();

        [Display(Name = "Created Date")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;
    }
}

