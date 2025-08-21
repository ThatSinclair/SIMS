public class Tutor
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public List<Course> Courses { get; set; } = new List<Course>();
    public int ExperienceYears { get; set; }
}