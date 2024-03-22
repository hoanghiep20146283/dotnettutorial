using Microsoft.AspNetCore.Mvc;
using CourseManagement.Models;
using CourseManagement.Entities;
using AutoMapper;

namespace CourseManagement.Services
{
    public class AuthorService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorService> _logger;
        private readonly AppDbContext _context;

        public AuthorService(ILogger<AuthorService> logger, AppDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public AuthorResponse getAuthorById(int authorId)
        {
            _context.Database.EnsureCreated();
            var author = (from a in _context.Authors
                          where (a.Id == authorId)
                          select a).FirstOrDefault();
            if (author == null)
                throw new KeyNotFoundException();

            return _mapper.Map<Author, AuthorResponse>(author);
        }

        public Author getAuthorByIdCircular(int authorId)
        {
            var author = (from a in _context.Authors
                          where (a.Id == authorId)
                          select a).FirstOrDefault();
            if (author == null)
                throw new KeyNotFoundException();

            return author;
        }

        public AuthorResponse Create([FromBody] AuthorRequest authorRequest)
        {
            var author = _mapper.Map<AuthorRequest, Author>(authorRequest);
            _context.Add(author);
            _context.SaveChanges();
            return _mapper.Map<Author, AuthorResponse>(author);
        }

        public AuthorResponse UpdateAuthor(int authorId, [FromBody] AuthorRequest authorRequest)
        {
            var author = (from a in _context.Authors
                          where (a.Id == authorId)
                          select a).FirstOrDefault();
            if (author == null)
                throw new KeyNotFoundException();

            author.Name = authorRequest.Name;
            author.UpdatedDate = DateTime.Now;

            _context.SaveChanges();

            return _mapper.Map<Author, AuthorResponse>(author);
        }

        public void DeleteAuthor(int authorId)
        {
            var author = (from a in _context.Authors
                          where (a.Id == authorId)
                          select a).FirstOrDefault();
            if (author == null)
                throw new KeyNotFoundException();

            _context.Remove(author);
            _context.SaveChanges();
        }
    }
}
