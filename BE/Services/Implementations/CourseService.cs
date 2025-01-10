using BE.DTOs.Course;
using BE.Models.Courses;
using BE.Repositories.Interfaces;
using BE.Services.Interfaces;

namespace BE.Services.Implementations;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;

    public CourseService(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task CreateNewCourse(CreateCourseDto dto)
    {
        var courseModel = new CoursesModel
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description
        };
        await _courseRepository.CreateCourseAsync(courseModel);
    }
}