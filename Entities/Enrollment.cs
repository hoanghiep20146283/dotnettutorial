using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseManagement.Entities
{
    [Table("enrollment")]
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public int UserId { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreationDate { set; get; }

        [DataType(DataType.Date)]
        public DateTime? UpdatedDate { set; get; }

        public virtual Course Course { get; set; }
    }
}
