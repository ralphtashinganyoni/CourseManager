using CourseManager.Application.Repository;
using CourseManager.Domain.Entities;
using CourseManager.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CourseManager.Infrastructure.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Course> GetByIdAsync(string id)
        {
            return await _context.Courses.FindAsync(id);
        }

        public async Task AddAsync(Course course)
        {
            await _context.Courses.AddAsync(course);
        }

        public async Task DeleteAsync(Course course)
        {
            _context.Courses.Remove(course);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IQueryable<Course> Query()
        {
            return _context.Courses.AsQueryable();
        }

        public async Task<int> GetAttendeeCountAsync(string courseId)
        {
            return await _context.Enrollments.Where(x => x.CourseId == new Guid(courseId)).CountAsync();
        }
    }
}