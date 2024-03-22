using AutoMapper;
using CourseManagement.Models;
using CourseManagement.Entities;
using Microsoft.AspNetCore.Identity;

namespace CourseManagement.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CourseRequest, Course>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => DateTime.Now))
                    .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.Now))
                    .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>srcMember != null));

            CreateMap<Course, CourseResponse>()
                    .AfterMap((src, dest, context) =>
                        dest.Author = context.Mapper.Map<AuthorInfo>(src.Author))
                    .AfterMap((src, dest, context) =>
                        dest.NumberOfEnrollments = src.Enrollments.Count)
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Course, CourseInfo>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Enrollment, EnrollmentInfo>()
                  .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<AuthorRequest, Author>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => DateTime.Now))
                    .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.Now))
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<EnrollmentRequest, Enrollment>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => DateTime.Now))
                    .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.Now))
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Author, AuthorResponse>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<IdentityUser, UserResponse>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserName))
                   .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Author, AuthorInfo>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Enrollment, EnrollmentResponse>()
                    .AfterMap((src, dest, context) =>
                        dest.Course = context.Mapper.Map<CourseInfo>(src.Course))
                    .AfterMap((src, dest, context) =>
                        dest.User = context.Mapper.Map<UserResponse>(src.User))
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}