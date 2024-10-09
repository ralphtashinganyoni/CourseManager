using CourseManager.UI.Entities;
using CourseManager.UI.DTOs;
using CourseManager.UI.Models;

namespace CourseManager.UI.Services
{
    public interface IAttendeeService
    {
        public Task<ProcessResponse<AttendeeModel>> GetAttendeeById(int id);
        public Task<ProcessResponse<List<AttendeeModel>>> GetAttendeesByCourseId(int id);
        public Task<ProcessResponse> CreateAttendee(AttendeeModel attendee);
        public Task<ProcessResponse> UpdateAttendee(int id, AttendeeModel attendee);
        public Task<ProcessResponse> DeleteAttendee(int id);
        public Task<bool> CheckIfAttendeeExists(int courseId, string email);
        public Task<ProcessResponse<AttendeeModel>> GetAttendee(int courseId, string email);

    }
}
