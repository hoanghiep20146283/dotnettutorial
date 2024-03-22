using Microsoft.AspNetCore.Mvc;
using CourseManagement.Models;
using CourseManagement.Entities;
using AutoMapper;
using CourseManagement.ErrorHandlers;
using CourseManagement;

namespace CourseManagement.Services
{
    public class EnrollmentService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<EnrollmentService> _logger;
        private readonly AppDbContext _context;

        public EnrollmentService(ILogger<EnrollmentService> logger, AppDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public EnrollmentResponse getEnrollmentById(int enrollmentId)
        {
            _context.Database.EnsureCreated();
            var Enrollment = (from c in _context.Enrollments
                          where (c.Id == enrollmentId)
                          select c).FirstOrDefault();
            if (Enrollment == null)
                throw new KeyNotFoundException();

            return _mapper.Map<Enrollment, EnrollmentResponse>(Enrollment);
        }

        public EnrollmentResponse Create([FromBody] EnrollmentRequest enrollmentRequest)
        {
            var existEnrollment = (from e in _context.Enrollments
                          where (e.CourseId == enrollmentRequest.CourseId & e.UserId == enrollmentRequest.UserId)
                          select e).FirstOrDefault();
            if (existEnrollment != null)
                throw new ConfictNameException($"Enrollment with course: {enrollmentRequest.CourseId} and user: {enrollmentRequest.UserId} already exists");

            var enrollment = _mapper.Map<EnrollmentRequest, Enrollment>(enrollmentRequest);
            _context.Add(enrollment);
            _context.SaveChanges();
            return _mapper.Map<Enrollment, EnrollmentResponse>(existEnrollment);
        }

        public EnrollmentResponse UpdateEnrollment(int enrollmentId, [FromBody] EnrollmentRequest enrollmentRequest)
        {
            var existingEnrollment = (from e in _context.Enrollments
                                      where (e.CourseId == enrollmentRequest.CourseId & e.UserId == enrollmentRequest.UserId & e.Id != enrollmentId)
                                      select e).FirstOrDefault();
            if (existingEnrollment != null)
                throw new ConfictNameException($"Enrollment with course: {enrollmentRequest.CourseId} and user: {enrollmentRequest.UserId} already exists");

            var Enrollment = (from c in _context.Enrollments
                          where (c.Id == enrollmentId)
                          select c).FirstOrDefault();
            if (Enrollment == null)
                throw new KeyNotFoundException();

            Enrollment.CourseId = enrollmentRequest.CourseId;
            Enrollment.UserId = enrollmentRequest.UserId;
            Enrollment.UpdatedDate = DateTime.Now;

            _context.SaveChanges();

            return _mapper.Map<Enrollment, EnrollmentResponse>(Enrollment);
        }

        public void DeleteEnrollment(int enrollmentId)
        {
            var enrollment = (from e in _context.Enrollments
                          where (e.Id == enrollmentId)
                          select e).FirstOrDefault();
            if (enrollment == null)
                throw new KeyNotFoundException();

            _context.Remove(enrollment);
            _context.SaveChanges();
        }
    }
}
