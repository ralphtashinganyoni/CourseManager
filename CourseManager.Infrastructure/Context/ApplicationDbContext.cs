using CourseManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CourseManager.Infrastructure.Context
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Enrollment>()
                .HasOne(e => e.Student)           // Navigation property in Enrollment
                .WithMany(u => u.Enrollments)      // Inverse navigation in User (note the plural)
                .HasForeignKey(e => e.StudentId)   // Foreign key in Enrollment
                .HasPrincipalKey(u => u.Id);   // Primary key in User

            base.OnModelCreating(builder);
        }


        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<User> Users { get; set; }


    }
}
