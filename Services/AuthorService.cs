using Microsoft.AspNetCore.Mvc;
using CourseManagement.Models;
using CourseManagement.Entities;
using CourseManagement.Repositories;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;

namespace CourseManagement.Services
{
    public class AuthorService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorService> _logger;
        private readonly AuthorRepository _authorRepository;
        public static readonly SemaphoreSlim _cacheSignal = new(1, 1);
        private readonly IMemoryCache _cache;

        public AuthorService(ILogger<AuthorService> logger, AppDbContext context, IMapper mapper, AuthorRepository authorRepository, IMemoryCache cache)
        {
            _logger = logger;
            _mapper = mapper;
            _authorRepository = authorRepository;
            _cache = cache;
        }

        public async IAsyncEnumerable<AuthorResponse> GetAllAuthorsAsync()
        {
            try
            {
                await _cacheSignal.WaitAsync();

                Author[] authors = (await _cache.GetOrCreateAsync("AllAuthors", _ =>
                        {
                            _logger.LogWarning("This should never happen!");
                            return Task.FromResult(Array.Empty<Author>());
                        }))!;

                foreach (Author author in authors)
                {
                    if (!default(Author).Equals(author))
                    {
                        yield return _mapper.Map<AuthorResponse>(author);
                    }
                }
            }
            finally
            {
                _cacheSignal.Release();
            }
        }

        public AuthorResponse getAuthorById(int authorId)
        {
            var author = _authorRepository.GetAuthorByID(authorId);
            if (author == null)
                throw new KeyNotFoundException();

            return _mapper.Map<Author, AuthorResponse>(author);
        }

        public Author getAuthorByIdCircular(int authorId)
        {
            var author = _authorRepository.GetAuthorByID(authorId);
            if (author == null)
                throw new KeyNotFoundException();

            return author;
        }

        public AuthorResponse Create([FromBody] AuthorRequest authorRequest)
        {
            var author = _mapper.Map<AuthorRequest, Author>(authorRequest);
            _authorRepository.InsertAuthor(author);
            return _mapper.Map<Author, AuthorResponse>(author);
        }

        public AuthorResponse UpdateAuthor(int authorId, [FromBody] AuthorRequest authorRequest)
        {
            var author = _authorRepository.GetAuthorByID(authorId);
            if (author == null)
                throw new KeyNotFoundException();

            author.Name = authorRequest.Name;
            author.UpdatedDate = DateTime.Now;

            _authorRepository.UpdateAuthor(author);

            return _mapper.Map<Author, AuthorResponse>(author);
        }

        public void DeleteAuthor(int authorId)
        {
            _authorRepository.DeleteAuthor(authorId);
        }
    }
}
