using CourseManager.UI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseManager.UI.DataAccessLayer
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Attendee> Attendees { get; set; }
    }
}
