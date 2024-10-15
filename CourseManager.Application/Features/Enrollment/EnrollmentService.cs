using CourseManager.Application.Repository;
using CourseManager.Domain.DTOs;
using CourseManager.Domain.Entities;
using CourseManager.Domain.Models;

namespace CourseManager.Application.Features.User
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        public EnrollmentService(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<bool> CheckIfEnrollmentExists(string courseId, string email)
        {
            return await _enrollmentRepository.EnrollmentExists(courseId, email);
        }

        public async Task<ProcessResponse<EnrollmentModel>> GetEnrollment(string courseId, string email)
        {
            var enrollment = await _enrollmentRepository.GetEnrollmentByCourseAndEmail(courseId, email);
            if (enrollment != null)
            {
                return new ProcessResponse<EnrollmentModel>
                {
                    IsSuccessful = true,
                    Data = enrollment
                };
            }

            return new ProcessResponse<EnrollmentModel>
            {
                IsSuccessful = false,
                Data = null
            };
        }

        public async Task<ProcessResponse> CreateEnrollment(EnrollmentModel enrollment)
        {
            try
            {
                await _enrollmentRepository.AddEnrollment(new Enrollment
                {
                    CourseId = new Guid(enrollment.CourseId),
                    StudentId = enrollment.Email,
                });
            }
            catch (Exception ex)
            {
                return new ProcessResponse
                {
                    IsSuccessful = false,
                    Message = "Creating enrollment failed"
                };
            }

            return new ProcessResponse
            {
                IsSuccessful = true,
                Message = "enrollment created"
            };
        }

        public async Task<ProcessResponse> DeleteEnrollment(string id)
        {
            var enrollmentToDelete = await _enrollmentRepository.GetEnrollmentById(id);
            if (enrollmentToDelete == null)
            {
                return new ProcessResponse
                {
                    IsSuccessful = false,
                    Message = "Selected enrollment cannot be found"
                };
            }
            try
            {
                await _enrollmentRepository.DeleteEnrollment(enrollmentToDelete);
            }
            catch
            {
                return new ProcessResponse
                {
                    IsSuccessful = false,
                    Message = "Deleting enrollment failed"
                };
            }

            return new ProcessResponse
            {
                IsSuccessful = true,
                Message = "enrollment successfully deleted"
            };
        }

        public async Task<ProcessResponse<EnrollmentModel>> GetEnrollmentById(string id)
        {
            var enrollment = await _enrollmentRepository.GetEnrollmentById(id);
            if (enrollment == null)
            {
                return new ProcessResponse<EnrollmentModel>
                {
                    IsSuccessful = false,
                    Message = "Selected enrollment cannot be found",
                    Data = null
                };
            }

            var result = new EnrollmentModel
            {
                Id = enrollment.Id.ToString(),
                Email = enrollment.StudentId,
                CourseId = enrollment.CourseId.ToString(),
            };

            return new ProcessResponse<EnrollmentModel>
            {
                IsSuccessful = true,
                Message = "Selected enrollment displayed",
                Data = result
            };
        }

        public async Task<ProcessResponse<List<EnrollmentModel>>> GetEnrollmentsByCourseId(string id)
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsByCourseId(id);
            return new ProcessResponse<List<EnrollmentModel>>
            {
                IsSuccessful = true,
                Message = "Enrollments for selected course displayed",
                Data = enrollments
            };
        }
    }
}
