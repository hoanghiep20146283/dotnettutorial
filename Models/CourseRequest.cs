using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CourseManagement.Models
{
    public class CourseRequest
    {
        [Required]
        [StringLength(100)]
        public string Title { set; get; }

        [Required]
        public int Duration { set; get; }

        [Required]
        public int AuthorId { set; get; }
    }
}
