using CourseManager.Domain.Common;

namespace CourseManager.Domain.Entities
{
    public class Course : BaseEntity
    {
        public string Name { get; set; }  = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset StartDateTime { get; set; }
        public int MaxNumberOfAttendees { get; set; }
        public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
