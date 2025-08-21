using System.ComponentModel.DataAnnotations;
using SIMSProject.Models;

public class Course
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    public int? AssignedTutorId { get; set; }
    public Tutor AssignedTutor { get; set; }
    public ICollection<Student> Students { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
}