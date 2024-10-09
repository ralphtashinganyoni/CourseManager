namespace CourseManager.UI.Models
{
    public class CoursesListModel
    {
        public int Id { get; set; }
        public string CourseTitle { get; set; } = null!;
        public DateTime CourseStartDateTime { get; set; }
        public string CourseCapacity { get; set; } = null!;
        public string CourseTeacher { get; set; } = null!;
        public bool IsCapacityFull { get; set; }
    }
}
