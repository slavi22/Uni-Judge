using BE.Business.Services.Interfaces;
using BE.Common.Exceptions;
using BE.DataAccess.Repositories.Interfaces;
using BE.DTOs.DTOs.Course.Requests;
using BE.DTOs.DTOs.Course.Responses;
using BE.Models.Models.Courses;
using Microsoft.AspNetCore.Http;

namespace BE.Business.Services.Implementations;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CourseService(ICourseRepository courseRepository, IUserRepository userRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _courseRepository = courseRepository;
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
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
                ProblemId = problem.ProblemId,
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

        var userEmail = _httpContextAccessor.HttpContext.User.Identity.Name;
        var user = await _userRepository.GetCurrentUserAsync(userEmail);

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

        var userEmail = _httpContextAccessor.HttpContext.User.Identity.Name;
        var currentUser = await _userRepository.GetCurrentUserAsync(userEmail);
        var newCourseModel = new CoursesModel
        {
            CourseId = dto.CourseId,
            Name = dto.Name,
            Description = dto.Description,
            Password = dto.Password,
            User = currentUser
        };
        await _courseRepository.CreateCourseAsync(newCourseModel);
        // sign up the creator of the course on creation
        var userCourse = new UserCourseModel
        {
            CourseId = newCourseModel.Id,
            UserId = currentUser.Id
        };
        await _courseRepository.SignUpForCourseAsync(newCourseModel, userCourse);
    }

    //TODO: add test
    public async Task<List<TeacherCoursesDto>> GetMyCreatedCoursesAsync()
    {
        var coursesList = new List<TeacherCoursesDto>();
        var userEmail = _httpContextAccessor.HttpContext.User.Identity.Name;
        var currentUser = await _userRepository.GetCurrentUserAsync(userEmail);
        var teacherCourses = await _courseRepository.GetTeacherCoursesAsync(currentUser.Id);
        foreach (var course in teacherCourses)
        {
            coursesList.Add(new TeacherCoursesDto
            {
                CourseId = course.CourseId, Name = course.Name, Description = course.Description
            });
        }

        return coursesList;
    }

    //TODO: add test
    public async Task<List<CourseDto>> GetAllCoursesAsync()
    {
        //TODO: add pagination
        var allCourses = await _courseRepository.GetAllCoursesAsync();
        var userEmail = _httpContextAccessor.HttpContext.User.Identity.Name;
        var currentUser = await _userRepository.GetCurrentUserAsync(userEmail);
        var coursesDto = new List<CourseDto>();
        foreach (var course in allCourses)
        {
            var userCourse =
                course.UserCourses.FirstOrDefault(uc => uc.CourseId == course.Id && uc.UserId == currentUser.Id);
            coursesDto.Add(new CourseDto
            {
                CourseId = course.CourseId,
                Name = course.Name,
                IsPasswordProtected = course.Password != null,
                UserIsEnrolled = course.UserCourses.Contains(userCourse)
            });
        }

        return coursesDto;
    }

    public async Task<List<EnrolledCourseDto>> GetEnrolledCoursesAsync()
    {
        var userEmail = _httpContextAccessor.HttpContext.User.Identity.Name;
        var currentUser = await _userRepository.GetCurrentUserAsync(userEmail);
        var enrolledCourses = await _courseRepository.GetAllEnrolledCoursesAsync(currentUser.Id);
        var enrolledCoursesDto = new List<EnrolledCourseDto>();
        foreach (var course in enrolledCourses)
        {
            enrolledCoursesDto.Add(new EnrolledCourseDto
            {
                CourseId = course.CourseId,
                Name = course.Name,
                Description = course.Description
            });
        }

        return enrolledCoursesDto;
    }

    public async Task<int> DeleteCourseByCourseId(string courseId)
    {
        var course = await _courseRepository.GetCourseByCourseIdAsync(courseId);
        if (course == null)
        {
            throw new CourseNotFoundException("Course not found.");
        }
        var result = await _courseRepository.DeleteCourseByCourseId(course);
        return result;
    }
}