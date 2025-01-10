using BE.DTOs.Course;

namespace BE.Services.Interfaces;

public interface ICourseService
{
    Task CreateNewCourse(CreateCourseDto dto);
}