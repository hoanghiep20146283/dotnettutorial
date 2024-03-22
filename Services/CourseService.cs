using Microsoft.AspNetCore.Mvc;
using CourseManagement.Models;
using CourseManagement.Entities;
using AutoMapper;
using CourseManagement.ErrorHandlers;

namespace CourseManagement.Services
{
    public class CourseService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CourseService> _logger;
        private readonly AppDbContext _context;

        public CourseService(ILogger<CourseService> logger, AppDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public CourseResponse getCourseById(int courseId)
        {
            _context.Database.EnsureCreated();
            var Course = (from c in _context.Courses
                          where (c.Id == courseId)
                          select c).FirstOrDefault();
            if (Course == null)
                throw new KeyNotFoundException();

            return _mapper.Map<Course, CourseResponse>(Course);
        }

        public CourseResponse Create([FromBody] CourseRequest courseRequest)
        {
            var course = (from c in _context.Courses
                          where (c.Title == courseRequest.Title)
                          select c).FirstOrDefault();
            if (course != null)
                throw new ConfictNameException($"Course with title: {courseRequest.Title} already exists");

            var newCourse = _mapper.Map<CourseRequest, Course>(courseRequest);
            _context.Add(newCourse);
            _context.SaveChanges();
            return _mapper.Map<Course, CourseResponse>(newCourse);
        }

        public CourseResponse UpdateCourse(int courseId, [FromBody] CourseRequest courseRequest)
        {
            var existingCourse = (from c in _context.Courses
                          where (c.Id != courseId & c.Title == courseRequest.Title)
                          select c).FirstOrDefault();
            if (existingCourse != null)
                throw new ConfictNameException($"Course with title: {courseRequest.Title} already exists");

            var course = (from c in _context.Courses
                          where (c.Id == courseId)
                          select c).FirstOrDefault();
            if (course == null)
                throw new KeyNotFoundException();

            course.Title= courseRequest.Title;
            course.UpdatedDate = DateTime.Now;

            _context.SaveChanges();

            return _mapper.Map<Course, CourseResponse>(course);
        }

        public ICollection<CourseResponse> getTopCourses()
        {
            return _context.Courses
                .Where(c => c.Enrollments.Count() >= 3)
                .OrderByDescending(c => c.Enrollments.Count())  
                .Take(3)
                .ToList()
                .Select(course => _mapper.Map<Course, CourseResponse>(course))                
                .ToList();
        }

        public void DeleteCourse(int courseId)
        {
            var course = (from c in _context.Courses
                          where (c.Id == courseId)
                          select c).FirstOrDefault();
            if (course == null)
                throw new KeyNotFoundException();

            _context.Remove(course);
            _context.SaveChanges();
        }
    }
}
