using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseManagement.Entities
{
    [Table("enrollment")]
    public class Enrollment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public string UserId { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreationDate { set; get; }

        [DataType(DataType.Date)]
        public DateTime? UpdatedDate { set; get; }

        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }

        [ForeignKey("UserId")]
        public virtual IdentityUser User { get; set; }

    }
}
