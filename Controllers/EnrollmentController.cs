using Microsoft.AspNetCore.Mvc;
using CourseManagement.Models;
using CourseManagement.Entities;
using CourseManagement.Services;
using CourseManagement;
using AutoMapper;

namespace EnrollmentManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EnrollmentController : ControllerBase
    {
        private readonly ILogger<EnrollmentController> _logger;

        private readonly AppDbContext _context;

        private readonly IMapper _mapper;

        private readonly EnrollmentService _enrollmentService;

        public EnrollmentController(EnrollmentService authorService, ILogger<EnrollmentController> logger, AppDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _enrollmentService = authorService;
        }

        [HttpGet]
        public ActionResult<List<EnrollmentResponse>> GetAllEnrollments()
        {
            var enrollments = (from e in _context.Enrollments select e).ToList();
            return enrollments.Select(enrollment => _mapper.Map<Enrollment, EnrollmentResponse>(enrollment)).ToList();
        }

        [HttpGet("{EnrollmentId}")]
        public ActionResult<EnrollmentResponse> getEnrollmentById(int enrollmentId)
        {
            return Ok(_enrollmentService.getEnrollmentById(enrollmentId));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public ActionResult<EnrollmentResponse> Create([FromBody] EnrollmentRequest enrollmentRequest)
        {
            return _enrollmentService.Create(enrollmentRequest);
        }

        [HttpPut("{EnrollmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<EnrollmentResponse> UpdateEnrollment(int enrollmentId, [FromBody] EnrollmentRequest enrollmentRequest)
        {
            return Ok(_enrollmentService.UpdateEnrollment(enrollmentId, enrollmentRequest));
        }

        [HttpDelete("{EnrollmentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteEnrollmentById(int enrollmentId)
        {
            _enrollmentService.DeleteEnrollment(enrollmentId);
            return NoContent();
        }
    }
}