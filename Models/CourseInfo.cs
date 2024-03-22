namespace CourseManagement.Models
{
    public class CourseInfo
    {
        public int Id { set; get; }
        public string Title { set; get; }

        public DateTime CreationDate { set; get; }

        public DateTime? UpdatedDate { set; get; }

        public int Duration { set; get; }
    }
}
