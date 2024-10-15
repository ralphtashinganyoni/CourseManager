using CourseManager.Domain.DTOs;
using CourseManager.Domain.Models;

namespace CourseManager.Application.Features.User
{
    public interface ICourseService
    {
        public Task<PageResponse<CoursesListModel>> GetAllCourses(int curPage, int pageSize, string searchString, DateTime? dateFrom, DateTime? dateTo);
        public Task<PageResponse<CoursesListModel>> GetUserEnrolledCourses(string userId, int curPage, int pageSize, string searchString, DateTime? dateFrom, DateTime? dateTo);
        public Task<PageResponse<CoursesListModel>> GetCoursesUserIsNotEnrolledIn(string userId, int curPage, int pageSize, string searchString, DateTime? dateFrom, DateTime? dateTo);
        public Task<ProcessResponse<CourseNewEditModel>> GetCourseByIdForCreateEdit(string id);
        public Task<ProcessResponse<CourseDetailsModel>> GetCourseDetailsById(string id);
        public Task<ProcessResponse> CreateCourse(CourseNewEditModel course);
        public Task<ProcessResponse> UpdateCourse(string id, CourseNewEditModel course);
        public Task<ProcessResponse> DeleteCourse(string id);
        public Task<bool> CheckIfCourseCapacityFull(string id);
    }
}
