using BE.DTOs.Course;
using BE.DTOs.Course.Requests;
using BE.Exceptions;
using BE.Models.Courses;
using BE.Repositories.Interfaces;
using BE.Services.Interfaces;

namespace BE.Services.Implementations;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IUserRepository _userRepository;

    public CourseService(ICourseRepository courseRepository, IUserRepository userRepository)
    {
        _courseRepository = courseRepository;
        _userRepository = userRepository;
    }

    public async Task<List<ViewCourseProblemDto>> GetProblemsForCourse(string courseId)
    {
        var course = await _courseRepository.GetCourseAndProblemsByIdAsync(courseId);
        var viewCourseProblemsList = new List<ViewCourseProblemDto>();
        foreach (var problem in course.Problems)
        {
            viewCourseProblemsList.Add(new ViewCourseProblemDto
            {
                ProblemId = problem.Id,
                Name = problem.Name,
                Description = problem.Description
            });
        }

        return viewCourseProblemsList;
    }

    public async Task<bool> SignUpForCourse(SignUpForCourseDto dto)
    {
        var course = await _courseRepository.GetCourseByIdAsync(dto.CourseId);
        if (course.Password != dto.Password)
        {
            throw new InvalidCoursePasswordException("Invalid course password entered.");
        }
        var user = await _userRepository.GetCurrentUserAsync();
        var userCourse = new UserCourseModel
        {
            CourseId = course.Id,
            UserId = user.Id
        };
        var result = await _courseRepository.SignUpForCourseAsync(course, userCourse);
        return result;
    }

    public async Task CreateNewCourse(CreateCourseDto dto)
    {
        var courseModel = new CoursesModel
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            Password = dto.Password
        };
        await _courseRepository.CreateCourseAsync(courseModel);
    }
}