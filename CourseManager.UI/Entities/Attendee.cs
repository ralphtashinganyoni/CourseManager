using System.ComponentModel.DataAnnotations;

namespace CourseManager.UI.Entities
{
    public class Attendee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(150, MinimumLength = 2)]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public int CourseId { get; set; }
    }
}
