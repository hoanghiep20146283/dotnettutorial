using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseManagement.Entities
{
    [Table("author")]
    public class Author
    {
        [Key]
        public int Id { set; get; }

        [MaxLength(100)]
        public string Name { set; get; }

        [DataType(DataType.Date)]
        public DateTime CreationDate { set; get; }

        [DataType(DataType.Date)]
        public DateTime? UpdatedDate { set; get; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
