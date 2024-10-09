using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Text;
using CourseManager.UI.DTOs;
using CourseManager.UI.Entities;
using CourseManager.UI.DataAccessLayer;
using CourseManager.UI.Models;

namespace CourseManager.UI.Services
{
    public class CourseService : ICourseService
    {
        private readonly ApplicationDbContext _context;
        private readonly byte[] salt = Encoding.ASCII.GetBytes("opakjogpkjdopajgkoirkjatki");

        public CourseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckCoursePIN(int id, string pin)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == id);
            var hashedPin = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: pin,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));

            return hashedPin == course.EditDeleteCoursePIN;
        }

        public async Task<bool> CheckIfCourseCapacityFull(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            var courseAttendeesCount = await _context.Attendees.Where(x => x.CourseId == id).CountAsync();
            return courseAttendeesCount + 1 <= course.MaxNumberOfAtendees;
        }

        public async Task<ProcessResponse> CreateCourse(CourseNewEditModel course)
        {
            Course courseToCreate = new Course
            {
                CourseTitle = course.CourseTitle,
                CourseDescription = course.CourseDescription,
                CourseTeacher = course.CourseTeacher,
                CourseTeacherEmail = course.CourseTeacherEmail,
                CourseStartDateTime = course.CourseStartDateTime,
                MaxNumberOfAtendees = course.MaxNumberOfAtendees,

                EditDeleteCoursePIN = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: course.EditDeleteCoursePIN,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8)),
            };

            try
            {
                _context.Courses.Add(courseToCreate);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
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

        public async Task<ProcessResponse> DeleteCourse(int id)
        {
            var courseToDelete = await _context.Courses.FindAsync(id);
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
                _context.Courses.Remove(courseToDelete);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
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
            var courseQuery = _context.Courses.Select(x => new CoursesListModel
            {
                Id = x.Id,
                CourseTitle = x.CourseTitle,
                CourseStartDateTime = x.CourseStartDateTime,
                CourseTeacher = x.CourseTeacher,
                CourseCapacity = $"{x.CourseAttendees.Count}/{x.MaxNumberOfAtendees}",
                IsCapacityFull = x.CourseAttendees.Count == x.MaxNumberOfAtendees,
            }).AsQueryable();

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
            var courseQuery = _context.Courses
                .Where(course => course.CourseAttendees.Any(attendee => attendee.Email == userId) // Filter by user enrollment
                )
                .Select(x => new CoursesListModel
                {
                    Id = x.Id,
                    CourseTitle = x.CourseTitle,
                    CourseStartDateTime = x.CourseStartDateTime,
                    CourseTeacher = x.CourseTeacher,
                    CourseCapacity = $"{x.CourseAttendees.Count}/{x.MaxNumberOfAtendees}",
                    IsCapacityFull = x.CourseAttendees.Count == x.MaxNumberOfAtendees,
                })
                .AsQueryable();

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
            // Query to select courses where the user is not enrolled
            var courseQuery = _context.Courses
                .Where(course => !course.CourseAttendees.Any(attendee => attendee.Email == userId))  // Exclude courses the user is enrolled in
                .Select(x => new CoursesListModel
                {
                    Id = x.Id,
                    CourseTitle = x.CourseTitle,
                    CourseStartDateTime = x.CourseStartDateTime,
                    CourseTeacher = x.CourseTeacher,
                    CourseCapacity = $"{x.CourseAttendees.Count}/{x.MaxNumberOfAtendees}",
                    IsCapacityFull = x.CourseAttendees.Count == x.MaxNumberOfAtendees,
                })
                .AsQueryable();

            // Apply search string filter if provided
            if (!string.IsNullOrEmpty(searchString))
            {
                courseQuery = courseQuery.Where(x => x.CourseTitle.Contains(searchString) || x.CourseTeacher.Contains(searchString));
            }

            // Apply date filter for 'from' date if provided
            if (dateFrom != null)
            {
                courseQuery = courseQuery.Where(x => x.CourseStartDateTime.Date >= dateFrom.Value.Date);
            }

            // Apply date filter for 'to' date if provided
            if (dateTo != null)
            {
                courseQuery = courseQuery.Where(x => x.CourseStartDateTime.Date <= dateTo.Value.Date);
            }

            // Return paginated response
            return await PageResponse<CoursesListModel>.CreateAsync(courseQuery, curPage, pageSize);
        }

        public async Task<PageResponse<CoursesListModel>> GetAllCourses(int curPage, int pageSize, string searchString)
        {
            var courseQuery = _context.Courses.Select(x => new CoursesListModel
            {
                Id = x.Id,
                CourseTitle = x.CourseTitle,
                CourseStartDateTime = x.CourseStartDateTime,
                CourseTeacher = x.CourseTeacher,
                CourseCapacity = $"{x.CourseAttendees.Count}/{x.MaxNumberOfAtendees}",
                IsCapacityFull = x.CourseAttendees.Count == x.MaxNumberOfAtendees,
            }).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                courseQuery = courseQuery.Where(x => x.CourseTitle.Contains(searchString) || x.CourseTeacher.Contains(searchString));
            }

            return await PageResponse<CoursesListModel>.CreateAsync(courseQuery, curPage, pageSize);
        }

        public async Task<ProcessResponse<CourseNewEditModel>> GetCourseByIdForCreateEdit(int id)
        {
            var course = await _context.Courses.Include(x => x.CourseAttendees).FirstOrDefaultAsync(x => x.Id == id);
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
                CourseTitle = course.CourseTitle,
                CourseDescription = course.CourseDescription,
                CourseTeacher = course.CourseTeacher,
                CourseTeacherEmail = course.CourseTeacherEmail,
                CourseStartDateTime = course.CourseStartDateTime,
                MaxNumberOfAtendees = course.MaxNumberOfAtendees,
                EditDeleteCoursePIN = string.Empty,
                CourseAttendees = new List<Attendee>(course.CourseAttendees),
            };

            return new ProcessResponse<CourseNewEditModel>
            {
                IsSuccessful = true,
                Message = "Selected course displayed",
                Data = result
            };
        }
        public async Task<ProcessResponse<CourseDetailsModel>> GetCourseDetailsById(int id)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (course == null)
            {
                return new ProcessResponse<CourseDetailsModel>
                {
                    IsSuccessful = false,
                    Message = "Selected course cannot be found",
                    Data = null
                };
            }

            var courseAttendeeCount = await _context.Attendees.Where(x => x.CourseId == id).CountAsync();
            var result = new CourseDetailsModel
            {
                CourseTitle = course.CourseTitle,
                CourseDescription = course.CourseDescription,
                CourseTeacher = course.CourseTeacher,
                CourseTeacherEmail = course.CourseTeacherEmail,
                CourseStartDateTime = course.CourseStartDateTime,
                CourseCapacity = $"{courseAttendeeCount}/{course.MaxNumberOfAtendees}"
            };
            return new ProcessResponse<CourseDetailsModel>
            {
                IsSuccessful = true,
                Message = "Selected course details displayed",
                Data = result
            };
        }

        public async Task<ProcessResponse> UpdateCourse(int id, CourseNewEditModel course)
        {
            var courseFromDb = await _context.Courses.FindAsync(id);
            if (courseFromDb == null)
            {
                return new ProcessResponse
                {
                    IsSuccessful = false,
                    Message = "Selected course cannot be found"
                };
            }

            if (await CheckCoursePIN(id, course.EditDeleteCoursePIN))
            {
                courseFromDb.CourseTitle = course.CourseTitle;
                courseFromDb.CourseDescription = course.CourseDescription;
                courseFromDb.CourseTeacher = course.CourseTeacher;
                courseFromDb.CourseTeacherEmail = course.CourseTeacherEmail;
                courseFromDb.CourseStartDateTime = course.CourseStartDateTime;
                courseFromDb.MaxNumberOfAtendees = course.MaxNumberOfAtendees;

                await _context.SaveChangesAsync();
            }
            else
            {
                return new ProcessResponse
                {
                    IsSuccessful = false,
                    Message = "Incorrect PIN! Course update failed"
                };
            }

            return new ProcessResponse
            {
                IsSuccessful = true,
                Message = "Selected course updated"
            };
        }
    }
}
