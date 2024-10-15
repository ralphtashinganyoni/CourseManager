using CourseManager.Domain.Common;
using System;

namespace CourseManager.Domain.Entities
{
    public class Enrollment : BaseEntity
    {
        public string StudentId { get; set; }
        public User Student { get; set; } = new User();
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = new Course();
    }
}
