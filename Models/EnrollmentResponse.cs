namespace CourseManagement.Models
{
    public class EnrollmentResponse
    {
        public int Id { set; get; }
        public int UserId { set; get; }

        public int CourseId { set; get; }

        public DateTime CreationDate { set; get; }

        public DateTime? UpdatedDate { set; get; }

        public CourseInfo Course { set; get; }
    }
}
