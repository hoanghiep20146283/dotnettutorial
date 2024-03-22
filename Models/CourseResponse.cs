namespace CourseManagement.Models
{
    public class CourseResponse
    {
        public int Id { set; get; }
        public string Title { set; get; }
        public int? NumberOfEnrollments { set; get; }

        public DateTime CreationDate { set; get; }

        public DateTime? UpdatedDate { set; get; }

        public int Duration { set; get; }

        public AuthorInfo Author { set; get; }
        public ICollection<EnrollmentResponse> Enrollments { set; get; }

    }
}
