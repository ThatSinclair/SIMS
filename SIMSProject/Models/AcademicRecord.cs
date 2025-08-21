using System.ComponentModel.DataAnnotations;

namespace SIMSProject.Models
{
    public class AcademicRecord
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public string Grade { get; set; } // Example additional property
    }
}