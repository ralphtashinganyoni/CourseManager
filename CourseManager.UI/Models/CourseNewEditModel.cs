using CourseManager.UI.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CourseManager.UI.Models
{
    public class CourseNewEditModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string CourseTitle { get; set; } = null!;
        [StringLength(4000)]
        public string? CourseDescription { get; set; }
        [Required]
        public DateTime CourseStartDateTime { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int MaxNumberOfAtendees { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string CourseTeacher { get; set; } = null!;
        [Required]
        [StringLength(150, MinimumLength = 2)]
        [EmailAddress]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Please enter a valid email.")]
        public string CourseTeacherEmail { get; set; } = null!;
        [Required]
        [PasswordPropertyText]
        [StringLength(150, MinimumLength = 4)]
        public string EditDeleteCoursePIN { get; set; } = null!;
        public List<Attendee> CourseAttendees { get; set; } = new List<Attendee>();
    }
}
