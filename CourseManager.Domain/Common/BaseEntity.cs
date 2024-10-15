using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CourseManager.Domain.Common
{
    public class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset? DateUpdated { get; set; }
        public DateTimeOffset? DateDeleted { get; set; }
    }
}
