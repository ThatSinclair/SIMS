using Microsoft.EntityFrameworkCore;

namespace SIMSProject.Data
{
    public class SIMSContext : DbContext
    {
        public SIMSContext(DbContextOptions<SIMSContext> options)
            : base(options)
        {
        }

        // Add DbSet<TEntity> properties here, for example:
        // public DbSet<Student> Students { get; set; }
        // public DbSet<Course> Courses { get; set; }
    }
}
