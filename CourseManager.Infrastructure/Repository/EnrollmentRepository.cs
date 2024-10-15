using CourseManager.Application.Repository;
using CourseManager.Domain.Entities;
using CourseManager.Domain.Models;
using CourseManager.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CourseManager.Infrastructure.Repository
{
    public class EnrollmentRepository : BaseRepository<Enrollment>, IEnrollmentRepository
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;

        }

        public async Task<bool> EnrollmentExists(string courseId, string studentId)
        {
            return await _context.Enrollments.AnyAsync(x => x.CourseId == new Guid(courseId) && x.StudentId == studentId);
        }

        public async Task<EnrollmentModel> GetEnrollmentByCourseAndEmail(string courseId, string studentId)
        {
            var enrollment = await _context.Enrollments.FirstOrDefaultAsync(x => x.CourseId == new Guid(courseId) && x.StudentId == studentId);
            if (enrollment == null) return null;

            return new EnrollmentModel
            {
                Id = enrollment.Id.ToString(),
                Email = enrollment.StudentId,
                CourseId = enrollment.CourseId.ToString(),
            };
        }

        public async Task<Enrollment> GetEnrollmentById(string id)
        {
            return await _context.Enrollments.FindAsync(id);
        }

        public async Task<List<EnrollmentModel>> GetEnrollmentsByCourseId(string courseId)
        {
            return await _context.Enrollments
                .Where(x => x.CourseId == new Guid(courseId))
                .Select(x => new EnrollmentModel
                {
                    Id = x.Id.ToString(),
                    Email = x.StudentId,
                    CourseId = x.CourseId.ToString(),
                }).ToListAsync();
        }

        public async Task AddEnrollment(Enrollment enrollment)
        {
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEnrollment(Enrollment enrollment)
        {
            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEnrollment(Enrollment enrollment)
        {
            _context.Enrollments.Update(enrollment);
            await _context.SaveChangesAsync();
        }
    }
}
