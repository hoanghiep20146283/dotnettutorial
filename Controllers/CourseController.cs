using Microsoft.AspNetCore.Mvc;
using CourseManagement.Models;
using CourseManagement.Entities;
using CourseManagement.Services;
using AutoMapper;

namespace CourseManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ILogger<CourseController> _logger;

        private readonly AppDbContext _context;

        private readonly IMapper _mapper;

        private readonly CourseService _courseService;

        public CourseController(CourseService authorService, ILogger<CourseController> logger, AppDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _courseService = authorService;
        }

        [HttpGet]
        public ActionResult<List<CourseResponse>> GetAllCourses()
        {
            var courses = (from c in _context.Courses select c).ToList();
            return courses.Select(course => _mapper.Map<Course, CourseResponse>(course)).ToList();
        }

        [HttpGet("{courseId}")]
        public ActionResult<CourseResponse> getCourseById(int courseId)
        {
            return Ok(_courseService.getCourseById(courseId));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public ActionResult<CourseResponse> Create([FromBody] CourseRequest courseRequest)
        {
            return _courseService.Create(courseRequest);
        }

        [HttpPut("{courseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CourseResponse> Updatecourse(int courseId, [FromBody] CourseRequest courseRequest)
        {
            return Ok(_courseService.UpdateCourse(courseId, courseRequest));
        }

        [HttpDelete("{courseId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CourseResponse> DeletecourseById(int courseId)
        {
            _courseService.DeleteCourse(courseId);
            return NoContent();
        }


        [HttpGet("/top")]
        public ActionResult<ICollection<CourseResponse>> getTopCourses()
        {
            return Ok(_courseService.getTopCourses());
        }
    }
}