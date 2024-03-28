using Moq;
using Microsoft.EntityFrameworkCore;
using CourseManagement.Repositories;
using CourseManagement.Entities;

namespace CourseManagement.Tests
{
    public class AuthorRepositoryTests
    {
        [Fact]
        public void GetAuthors_Returns_AllAuthors()
        {
            // Arrange
            var data = new List<Author>
            {
                new Author { Id = 1, Name = "Author 1" },
                new Author { Id = 2, Name = "Author 2" },
                new Author { Id = 3, Name = "Author 3" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Author>>();
            mockSet.As<IQueryable<Author>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Author>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Author>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Author>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<AppDbContext>();
            mockContext.Setup(c => c.Authors).Returns(mockSet.Object);

            var repository = new AuthorRepository(mockContext.Object);

            // Act
            var authors = repository.GetAuthors();

            // Assert
            Assert.Equal(3, authors.Count());
        }

        [Fact]
        public void GetAuthorById_Returns_Author_When_Found()
        {
            // Arrange
            var authorId = 1;
            var author = new Author { Id = authorId, Name = "Author 1" };

            var mockSet = new Mock<DbSet<Author>>();
            mockSet.Setup(m => m.Find(authorId)).Returns(author);

            var mockContext = new Mock<AppDbContext>();
            mockContext.Setup(c => c.Authors).Returns(mockSet.Object);

            var repository = new AuthorRepository(mockContext.Object);

            // Act
            var result = repository.GetAuthorByID(authorId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(authorId, result.Id);
        }

        [Fact]
        public void GetAuthorById_Returns_Null_When_Not_Found()
        {
            // Arrange
            var authorId = 1;
            Author author = null;

            var mockSet = new Mock<DbSet<Author>>();
            mockSet.Setup(m => m.Find(authorId)).Returns(author);

            var mockContext = new Mock<AppDbContext>();
            mockContext.Setup(c => c.Authors).Returns(mockSet.Object);

            var repository = new AuthorRepository(mockContext.Object);

            // Act
            var result = repository.GetAuthorByID(authorId);

            // Assert
            Assert.Null(result);
        }

        // Similarly, write tests for other methods like InsertAuthor, UpdateAuthor, DeleteAuthor, Save, etc.
    }
}
