using Moq;
using Microsoft.Extensions.Logging;
using CourseManagement.Services;
using CourseManagement.Models;
using AutoMapper;
using CourseManagement.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace CourseManagement.Tests
{
    public class AuthorServiceTests : IClassFixture<AuthorServiceFixture>
    {
        private readonly AuthorService _service;
        private readonly Mock<IMemoryCache> _memoryCacheMock;

        public AuthorServiceTests(AuthorServiceFixture fixture)
        {
            _service = fixture.Service;
            _memoryCacheMock = fixture.MemoryCacheMock;
        }

        [Fact]
        public async Task GetAllAuthorsAsync_Returns_Authors_From_Cache_When_Available()
        {
            // Arrange
            var sampleAuthor = new AuthorResponse { Id = 1, Name = "Sample Author" };
            var cachedAuthors = new List<AuthorResponse> { sampleAuthor };
            var cacheEntryMock = new Mock<ICacheEntry>();
            cacheEntryMock.Setup(entry => entry.Value).Returns(cachedAuthors);

            _memoryCacheMock
                .Setup(mc => mc.CreateEntry(It.IsAny<object>()))
                .Returns(cacheEntryMock.Object);

            // Act
            var result = await _service.GetAllAuthorsAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal(sampleAuthor.Id, result[0].Id);
            Assert.Equal(sampleAuthor.Name, result[0].Name);
        }

        [Fact]
        public async Task GetAllAuthorsAsync_Returns_Empty_From_Cache()
        {
            // Act
            var result = await _service.GetAllAuthorsAsync();

            // Assert
            Assert.Empty(result);
        }
    }

    public class AuthorServiceFixture
    {
        public AuthorService Service { get; private set; }
        public Mock<IMemoryCache> MemoryCacheMock { get; private set; }

        public AuthorServiceFixture()
        {
            var loggerMock = new Mock<ILogger<AuthorService>>();
            var mapperMock = new Mock<IMapper>();
            var authorRepositoryMock = new Mock<AuthorRepository>();
            MemoryCacheMock = new Mock<IMemoryCache>();

            Service = new AuthorService(loggerMock.Object, null, mapperMock.Object, authorRepositoryMock.Object, MemoryCacheMock.Object);
        }
    }
}
