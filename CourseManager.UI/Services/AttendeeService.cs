using CourseManager.UI.DataAccessLayer;
using CourseManager.UI.DTOs;
using CourseManager.UI.Entities;
using CourseManager.UI.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManager.UI.Services
{
    public class AttendeeService : IAttendeeService
    {
        private readonly ApplicationDbContext _context;
        public AttendeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckIfAttendeeExists(int courseId, string email)
        {
            var courseAttendees = await _context.Attendees.Where(x => x.CourseId == courseId).ToListAsync();
            return courseAttendees.Any(x => x.Email == email);
        }

        public async Task<ProcessResponse<AttendeeModel>> GetAttendee(int courseId, string email)
        {
            var courseAttendees = await _context.Attendees.Where(x => x.CourseId == courseId).ToListAsync();
            if (courseAttendees.Any())
            {
                var attendees = courseAttendees.Select(x => new AttendeeModel
                {
                    Id = x.Id,
                    Email = x.Email,
                    CourseId = x.CourseId
                }).ToList();
                return new ProcessResponse<AttendeeModel>
                {
                    IsSuccessful = true,
                    Data = attendees.FirstOrDefault()
                };
            }

            return new ProcessResponse<AttendeeModel>
            {
                IsSuccessful = false,
                Data = null
            };
        }

        public async Task<ProcessResponse> CreateAttendee(AttendeeModel attendee)
        {
            Attendee attendeeToCreate = new Attendee
            {
                Email = attendee.Email,
                CourseId = attendee.CourseId,
            };

            try
            {
                _context.Attendees.Add(attendeeToCreate);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new ProcessResponse
                {
                    IsSuccessful = false,
                    Message = "Creating attendee failed"
                };
            }

            return new ProcessResponse
            {
                IsSuccessful = true,
                Message = "Attendee created"
            };
        }

        public async Task<ProcessResponse> DeleteAttendee(int id)
        {
            var attendeeToDelete = await _context.Attendees.FindAsync(id);
            if (attendeeToDelete == null)
            {
                return new ProcessResponse
                {
                    IsSuccessful = false,
                    Message = "Selected attendee cannot be found"
                };
            }
            try
            {
                _context.Attendees.Remove(attendeeToDelete);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return new ProcessResponse
                {
                    IsSuccessful = false,
                    Message = "Deleting attendee failed"
                };
            }

            return new ProcessResponse
            {
                IsSuccessful = true,
                Message = "Attendee successfully deleted"
            };

        }

        public async Task<ProcessResponse<AttendeeModel>> GetAttendeeById(int id)
        {
            var attendee = await _context.Attendees.FindAsync(id);
            if (attendee == null)
            {
                return new ProcessResponse<AttendeeModel>
                {
                    IsSuccessful = false,
                    Message = "Selected attendee cannot be found",
                    Data = null
                };
            }

            var result = new AttendeeModel
            {
                Id = attendee.Id,
                Email = attendee.Email,
                CourseId = attendee.CourseId
            };

            return new ProcessResponse<AttendeeModel>
            {
                IsSuccessful = true,
                Message = "Selected attendee displayed",
                Data = result
            };
        }

        public async Task<ProcessResponse<List<AttendeeModel>>> GetAttendeesByCourseId(int id)
        {
            var attendees = await _context.Attendees.Where(x => x.CourseId == id).Select(x => new AttendeeModel
            {
                Id = x.Id,
                Email = x.Email,
                CourseId = x.CourseId
            }).ToListAsync();

            return new ProcessResponse<List<AttendeeModel>>
            {
                IsSuccessful = true,
                Message = "Attendees for selected course displayed",
                Data = attendees
            };
        }

        public async Task<ProcessResponse> UpdateAttendee(int id, AttendeeModel attendee)
        {
            var attendeeToUpdate = await _context.Attendees.FindAsync(id);
            if (attendeeToUpdate == null)
            {
                return new ProcessResponse
                {
                    IsSuccessful = false,
                    Message = "Selected attendee cannot be found"
                };
            }

            try
            {
                attendeeToUpdate.Email = attendee.Email;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new ProcessResponse
                {
                    IsSuccessful = false,
                    Message = "Selected attendee update failed"
                };
            }

            return new ProcessResponse
            {
                IsSuccessful = true,
                Message = "Attendee successfully updated"
            };
        }
    }
}
