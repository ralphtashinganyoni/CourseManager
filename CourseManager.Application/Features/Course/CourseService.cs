using CourseManager.Application.Repository;
using CourseManager.Domain.DTOs;
using CourseManager.Domain.Entities;
using CourseManager.Domain.Models;

namespace CourseManager.Application.Features.User
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<bool> CheckIfCourseCapacityFull(string id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            var courseAttendeesCount = await _courseRepository.GetAttendeeCountAsync(id);
            return courseAttendeesCount + 1 <= course.MaxNumberOfAttendees;
        }

        public async Task<ProcessResponse> CreateCourse(CourseNewEditModel course)
        {
            Course courseToCreate = new Course
            {
                Name = course.CourseTitle,
                Code = course.CourseDescription,
                Description = course.CourseTeacher,
                DateCreated = DateTimeOffset.Now,
                StartDateTime = course.CourseStartDateTime,
                MaxNumberOfAttendees = course.MaxNumberOfAttendees,
            };

            try
            {
                await _courseRepository.AddAsync(courseToCreate);
                await _courseRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return new ProcessResponse
                {
                    IsSuccessful = false,
                    Message = "Failed to create course"
                };
            }

            return new ProcessResponse
            {
                IsSuccessful = true,
                Message = "Course successfully created"
            };
        }

        public async Task<ProcessResponse> DeleteCourse(string id)
        {
            var courseToDelete = await _courseRepository.GetByIdAsync(id);
            if (courseToDelete == null)
            {
                return new ProcessResponse
                {
                    IsSuccessful = false,
                    Message = "Selected course cannot be found"
                };
            }
            try
            {
                await _courseRepository.DeleteAsync(courseToDelete);
                await _courseRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return new ProcessResponse
                {
                    IsSuccessful = false,
                    Message = "Selected course cannot be deleted"
                };
            }

            return new ProcessResponse
            {
                IsSuccessful = true,
                Message = "Selected course deleted"
            };
        }

        public async Task<PageResponse<CoursesListModel>> GetAllCourses(int curPage, int pageSize, string searchString, DateTime? dateFrom, DateTime? dateTo)
        {
            var courseQuery = _courseRepository.Query().Select(x => new CoursesListModel
            {
                Id = x.Id,
                CourseTitle = x.Name,
                CourseStartDateTime = x.StartDateTime,
                CourseCapacity = $"{x.Enrollments.Count}/{x.MaxNumberOfAttendees}",
                IsCapacityFull = x.Enrollments.Count == x.MaxNumberOfAttendees,
            });

            if (!string.IsNullOrEmpty(searchString))
            {
                courseQuery = courseQuery.Where(x => x.CourseTitle.Contains(searchString) || x.CourseTeacher.Contains(searchString));
            }

            if (dateFrom != null)
            {
                courseQuery = courseQuery.Where(x => x.CourseStartDateTime.Date >= dateFrom.Value.Date);
            }

            if (dateTo != null)
            {
                courseQuery = courseQuery.Where(x => x.CourseStartDateTime.Date <= dateTo.Value.Date);
            }

            return await PageResponse<CoursesListModel>.CreateAsync(courseQuery, curPage, pageSize);
        }

        public async Task<PageResponse<CoursesListModel>> GetUserEnrolledCourses(string userId, int curPage, int pageSize, string searchString, DateTime? dateFrom, DateTime? dateTo)
        {
            var courseQuery = _courseRepository.Query()
                .Where(course => course.Enrollments.Any(enrollment => enrollment.StudentId == userId))
                .Select(x => new CoursesListModel
                {
                    Id = x.Id,
                    CourseTitle = x.Name,
                    CourseStartDateTime = x.StartDateTime,
                    CourseCapacity = $"{x.Enrollments.Count}/{x.MaxNumberOfAttendees}",
                    IsCapacityFull = x.Enrollments.Count == x.MaxNumberOfAttendees,
                });

            if (!string.IsNullOrEmpty(searchString))
            {
                courseQuery = courseQuery.Where(x => x.CourseTitle.Contains(searchString) || x.CourseTeacher.Contains(searchString));
            }

            if (dateFrom != null)
            {
                courseQuery = courseQuery.Where(x => x.CourseStartDateTime.Date >= dateFrom.Value.Date);
            }

            if (dateTo != null)
            {
                courseQuery = courseQuery.Where(x => x.CourseStartDateTime.Date <= dateTo.Value.Date);
            }

            return await PageResponse<CoursesListModel>.CreateAsync(courseQuery, curPage, pageSize);
        }

        public async Task<PageResponse<CoursesListModel>> GetCoursesUserIsNotEnrolledIn(string userId, int curPage, int pageSize, string searchString, DateTime? dateFrom, DateTime? dateTo)
        {
            var courseQuery = _courseRepository.Query()
                .Where(course => !course.Enrollments.Any(enrollment => enrollment.StudentId == userId))
                .Select(x => new CoursesListModel
                {
                    Id = x.Id,
                    CourseTitle = x.Name,
                    CourseStartDateTime = x.StartDateTime,
                    CourseCapacity = $"{x.Enrollments.Count}/{x.MaxNumberOfAttendees}",
                    IsCapacityFull = x.Enrollments.Count == x.MaxNumberOfAttendees,
                });

            if (!string.IsNullOrEmpty(searchString))
            {
                courseQuery = courseQuery.Where(x => x.CourseTitle.Contains(searchString) || x.CourseTeacher.Contains(searchString));
            }

            if (dateFrom != null)
            {
                courseQuery = courseQuery.Where(x => x.CourseStartDateTime.Date >= dateFrom.Value.Date);
            }

            if (dateTo != null)
            {
                courseQuery = courseQuery.Where(x => x.CourseStartDateTime.Date <= dateTo.Value.Date);
            }

            return await PageResponse<CoursesListModel>.CreateAsync(courseQuery, curPage, pageSize);
        }

        public async Task<ProcessResponse<CourseNewEditModel>> GetCourseByIdForCreateEdit(string id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                return new ProcessResponse<CourseNewEditModel>
                {
                    IsSuccessful = false,
                    Message = "Selected course cannot be found",
                    Data = null
                };
            }

            var result = new CourseNewEditModel
            {
                Id = course.Id,
                CourseTitle = course.Name,
                CourseDescription = course.Description,
                CourseStartDateTime = DateTime.Parse(course.StartDateTime.ToString()),
                MaxNumberOfAttendees = course.MaxNumberOfAttendees,
                Enrollments = new List<Enrollment>(course.Enrollments),
            };

            return new ProcessResponse<CourseNewEditModel>
            {
                IsSuccessful = true,
                Message = "Selected course displayed",
                Data = result
            };
        }

        public async Task<ProcessResponse<CourseDetailsModel>> GetCourseDetailsById(string id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                return new ProcessResponse<CourseDetailsModel>
                {
                    IsSuccessful = false,
                    Message = "Selected course cannot be found",
                    Data = null
                };
            }

            var courseAttendeeCount = await _courseRepository.GetAttendeeCountAsync(id);
            var result = new CourseDetailsModel
            {
                CourseTitle = course.Name,
                CourseDescription = course.Description,
                CourseStartDateTime = course.StartDateTime,
                CourseCapacity = $"{courseAttendeeCount}/{course.MaxNumberOfAttendees}"
            };
            return new ProcessResponse<CourseDetailsModel>
            {
                IsSuccessful = true,
                Message = "Selected course details displayed",
                Data = result
            };
        }

        public async Task<ProcessResponse> UpdateCourse(string id, CourseNewEditModel course)
        {
            var courseFromDb = await _courseRepository.GetByIdAsync(id);
            if (courseFromDb == null)
            {
                return new ProcessResponse
                {
                    IsSuccessful = false,
                    Message = "Selected course cannot be found"
                };
            }
            courseFromDb.Name = course.CourseTitle;
            courseFromDb.Description = course.CourseDescription;
            courseFromDb.Code = course.CourseTeacher;
            courseFromDb.DateUpdated = DateTimeOffset.Now;
            courseFromDb.StartDateTime = course.CourseStartDateTime;
            courseFromDb.MaxNumberOfAttendees = course.MaxNumberOfAttendees;

            await _courseRepository.SaveChangesAsync();

            return new ProcessResponse
            {
                IsSuccessful = true,
                Message = "Selected course updated"
            };
        }
    }
}