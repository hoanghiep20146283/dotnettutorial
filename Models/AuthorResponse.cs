namespace CourseManagement.Models
{
    public class AuthorResponse
    {
        public int Id { set; get; }

        public string Name { set; get; }

        public DateTime CreationDate { set; get; }

        public DateTime? UpdatedDate { set; get; }
        public ICollection<CourseInfo> Courses { get; set; }
    }
}
