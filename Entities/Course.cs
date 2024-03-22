using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseManagement.Entities
{
    [Table("course")]
    public class Course
    {
        [Key]
        public int Id { set; get; }

        [MaxLength(100)]
        public string Title { set; get; }

        [DataType(DataType.Date)]
        public DateTime CreationDate { set; get; }

        [DataType(DataType.Date)]
        public DateTime? UpdatedDate { set; get; }

        public int Duration { set; get; }

        public int AuthorId { set; get; }   

        [ForeignKey("AuthorId")]
        [Required]
        public virtual Author Author { set; get; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }

    }
}
