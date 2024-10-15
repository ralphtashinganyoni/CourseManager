using CourseManager.Domain.DTOs;
using CourseManager.Domain.Models;

namespace CourseManager.Application.Features.User
{
    public interface IEnrollmentService
    {
        public Task<ProcessResponse<EnrollmentModel>> GetEnrollmentById(string id);
        public Task<ProcessResponse<List<EnrollmentModel>>> GetEnrollmentsByCourseId(string id);
        public Task<ProcessResponse> CreateEnrollment(EnrollmentModel enrollment);
        public Task<ProcessResponse> DeleteEnrollment(string id);
        public Task<bool> CheckIfEnrollmentExists(string courseId, string email);
        public Task<ProcessResponse<EnrollmentModel>> GetEnrollment(string courseId, string email);
    }
}
