using Microsoft.AspNetCore.Mvc;
using CourseManagement.Models;
using CourseManagement.Entities;
using CourseManagement.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Memory;

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

        private readonly IMemoryCache _memoryCache;

        private const int MillisecondsDelayAfterAdd = 50000;
        private const int MillisecondsAbsoluteExpiration = 75000;

        public AuthorController(AuthorService authorService, ILogger<AuthorController> logger, AppDbContext context, IMapper mapper, IMemoryCache memoryCache)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _authorService = authorService;
            _memoryCache = memoryCache;
        }

        static void OnPostEviction(object key, object value, EvictionReason reason, object state)
        {
            Console.WriteLine($"{key} was evicted for {reason}.");
        }

        [HttpGet]
        public async Task<List<AuthorResponse>> GetAllAuthors()
        {
            return await _authorService.GetAllAuthorsAsync();
        }

        [HttpGet("{authorId}")]
        public ActionResult<AuthorResponse> getAuthorById(int authorId)
        {
            if (_memoryCache.TryGetValue(authorId, out AuthorResponse authorResponse))
            {
                Console.WriteLine($"{authorResponse} is still in cache.");
                return Ok(authorResponse);
            }

            MemoryCacheEntryOptions options = new() {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(MillisecondsAbsoluteExpiration)
            };

            AuthorResponse author = _authorService.getAuthorById(authorId);
            options.RegisterPostEvictionCallback(OnPostEviction);
            _memoryCache.Set(authorId, author, options);
            Console.WriteLine($"{author} was cached.");
            return Ok(author);
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