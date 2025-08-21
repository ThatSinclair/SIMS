using Microsoft.EntityFrameworkCore;
using SIMSProject.Models;

namespace SIMSProject.Data
{
    public class SIMSContext : DbContext
    {
        public SIMSContext(DbContextOptions<SIMSContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<AcademicRecord> AcademicRecords { get; set; }
        // Add other DbSet properties as needed, e.g. Tutors, Users, etc.
    }
}