using CourseManagement.Entities;

namespace CourseManagement.Repositories
{
    public interface IAuthorRepository : IDisposable
    {
        IEnumerable<Author> GetAuthors();
        Author GetAuthorByID(int authorId);
        void InsertAuthor(Author author);
        void DeleteAuthor(int authorId);
        void UpdateAuthor(Author author);
        void Save();
    }
}