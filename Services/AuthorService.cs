using Microsoft.AspNetCore.Mvc;
using CourseManagement.Models;
using CourseManagement.Entities;
using CourseManagement.Repositories;
using AutoMapper;

namespace CourseManagement.Services
{
    public class AuthorService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorService> _logger;
        private readonly AuthorRepository _authorRepository;

        public AuthorService(ILogger<AuthorService> logger, AppDbContext context, IMapper mapper, AuthorRepository authorRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _authorRepository = authorRepository;
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
