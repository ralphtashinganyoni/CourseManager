using CourseManager.Domain.Entities;

namespace CourseManager.Application.Repository
{
    public interface ICourseRepository
    {
        Task<Course> GetByIdAsync(string id);
        Task AddAsync(Course course);
        Task DeleteAsync(Course course);
        Task SaveChangesAsync();
        IQueryable<Course> Query();
        Task<int> GetAttendeeCountAsync(string courseId);
    }
}