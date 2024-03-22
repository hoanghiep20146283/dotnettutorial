using Microsoft.AspNetCore.Mvc;
using CourseManagement.Models;
using CourseManagement.Entities;
using CourseManagement.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CourseManagement.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Member,Admin")]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly ILogger<AuthorController> _logger;

        private readonly AppDbContext _context;

        private readonly IMapper _mapper;

        private readonly AuthorService _authorService;

        public AuthorController(AuthorService authorService, ILogger<AuthorController> logger, AppDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _authorService = authorService;
        }

        [HttpGet]
        public ActionResult<List<AuthorResponse>> GetAllAuthors()
        {
            var authors = (from a in _context.Authors select a).ToList();
            return authors.Select(author => _mapper.Map<Author, AuthorResponse>(author)).ToList();
        }

        [HttpGet("{authorId}")]
        public ActionResult<AuthorResponse> getAuthorById(int authorId)
        {
            return Ok(_authorService.getAuthorById(authorId));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public ActionResult<AuthorResponse> Create([FromBody] AuthorRequest authorRequest)
        {
            return _authorService.Create(authorRequest);
        }

        [HttpPut("{authorId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<AuthorResponse> UpdateAuthor(int authorId, [FromBody] AuthorRequest authorRequest)
        {
            return Ok(_authorService.UpdateAuthor(authorId, authorRequest));
        }

        [HttpDelete("{authorId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CourseResponse> DeleteAuthorById(int authorId)
        {
            _authorService.DeleteAuthor(authorId);
            return NoContent();
        }

        [HttpGet("/circular/{authorId}")]
        public ActionResult<Author> getAuthorByIdCircular(int authorId)
        {
            return Ok(_authorService.getAuthorByIdCircular(authorId));
        }

    }
}