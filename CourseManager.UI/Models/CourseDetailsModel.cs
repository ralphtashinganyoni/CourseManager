namespace CourseManager.UI.Models
{
    public class CourseDetailsModel
    {
        public string CourseTitle { get; set; } = null!;
        public string? CourseDescription { get; set; }
        public DateTime CourseStartDateTime { get; set; }
        public string CourseCapacity { get; set; } = null!;
        public string CourseTeacher { get; set; } = null!;
        public string CourseTeacherEmail { get; set; } = null!;
    }
}
