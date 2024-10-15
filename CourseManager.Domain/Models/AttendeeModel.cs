using System.ComponentModel.DataAnnotations;

namespace CourseManager.Domain.Models
{
    public class EnrollmentModel
    {
        public string Id { get; set; }
        [Required]
        [StringLength(150, MinimumLength = 2)]
        [EmailAddress]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Please enter a valid email.")]
        public string Email { get; set; } = null!;
        [Required]
        public string CourseId { get; set; }
    }
}
