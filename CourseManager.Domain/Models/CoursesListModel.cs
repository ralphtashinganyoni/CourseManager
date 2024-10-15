namespace CourseManager.Domain.Models
{
    public class CoursesListModel
    {
        public Guid Id { get; set; }
        public string CourseTitle { get; set; } = null!;
        public DateTimeOffset CourseStartDateTime { get; set; }
        public string CourseCapacity { get; set; } = null!;
        public string CourseTeacher { get; set; } = null!;
        public bool IsCapacityFull { get; set; }
    }
}
