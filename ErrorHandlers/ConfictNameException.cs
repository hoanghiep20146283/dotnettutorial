namespace CourseManagement.ErrorHandlers
{
    public class ConfictNameException : Exception
    {
        public ConfictNameException(string message) : base(message)
        {
        }
    }
}
