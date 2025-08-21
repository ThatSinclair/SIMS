public class SIMSContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<AcademicRecord> AcademicRecords { get; set; }
    public DbSet<Tutor> Tutors { get; set; } // <-- Add this property
}