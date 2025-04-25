using BE.Business.Services.Interfaces;
using BE.Common.Exceptions;
using BE.DataAccess.Repositories.Interfaces;
using BE.DTOs.DTOs.Course.Requests;
using BE.DTOs.DTOs.Course.Responses;
using BE.Models.Models.Courses;

namespace BE.Business.Services.Implementations;

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
        // We get all the problems for a course and return them in a list of ViewCourseProblemDto
        var course = await _courseRepository.GetCourseAndProblemsByIdAsync(courseId);
        if (course == null)
        {
            return null;
        }
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
        // We sign up a user for a course. Some courses require a password to sign up which is checked here.
        var course = await _courseRepository.GetCourseByCourseIdAsync(dto.CourseId);
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
        // We create a new course with the provided data. Only users with have the teacher role can create courses.
        var course = await _courseRepository.GetCourseByCourseIdAsync(dto.CourseId);
        if (course != null)
        {
            throw new DuplicateCourseIdException($"A course with the given ID - '{course.CourseId}' already exists.");
        }
        var newCourseModel = new CoursesModel
        {
            CourseId = dto.CourseId,
            Name = dto.Name,
            Description = dto.Description,
            Password = dto.Password
        };
        await _courseRepository.CreateCourseAsync(newCourseModel);
    }
}