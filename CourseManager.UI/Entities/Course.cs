using System.ComponentModel.DataAnnotations;

namespace CourseManager.UI.Entities
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string CourseTitle { get; set; } = null!;
        [StringLength(4000)]
        public string? CourseDescription { get; set; }
        [Required]
        public DateTime CourseStartDateTime { get; set; }
        public List<Attendee> CourseAttendees { get; set; } = new List<Attendee>();
        [Required]
        [Range(1, int.MaxValue)]
        public int MaxNumberOfAtendees { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string CourseTeacher { get; set; } = null!;
        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string CourseTeacherEmail { get; set; } = null!;
    }
}
