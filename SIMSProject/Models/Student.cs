using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIMSProject.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string StudentId { get; set; }
        public ICollection<Course> Courses { get; set; } // For many-to-many with Course
        public ICollection<AcademicRecord> AcademicRecords { get; set; } // Added for one-to-many with AcademicRecord

        public Student()
        {
            Courses = new List<Course>();
            AcademicRecords = new List<AcademicRecord>();
        }
    }
}