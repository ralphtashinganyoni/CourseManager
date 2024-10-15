using CourseManager.Domain.Entities;
using CourseManager.Domain.Models;

namespace CourseManager.Application.Repository
{
    public interface IEnrollmentRepository : IBaseRepository<Enrollment>
    {
        Task<bool> EnrollmentExists(string courseId, string email);
        Task<EnrollmentModel> GetEnrollmentByCourseAndEmail(string courseId, string email);
        Task<Enrollment> GetEnrollmentById(string id);
        Task<List<EnrollmentModel>> GetEnrollmentsByCourseId(string courseId);
        Task AddEnrollment(Enrollment enrollment);
        Task DeleteEnrollment(Enrollment enrollment);
        Task UpdateEnrollment(Enrollment enrollment);
    }
}
