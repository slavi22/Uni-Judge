using BE.DTOs.DTOs.Course.Requests;
using BE.DTOs.DTOs.Course.Responses;

namespace BE.Business.Services.Interfaces;

public interface ICourseService
{
    /// <summary>
    /// Retrieves the problems for a specific course.
    /// </summary>
    /// <param name="courseId">The ID of the course to retrieve problems for</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of course problem DTOs</returns>
    Task<List<ViewCourseProblemDto>> GetProblemsForCourse(string courseId);

    /// <summary>
    /// Signs up a user for a course.
    /// </summary>
    /// <param name="dto">The sign-up details</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the sign-up was successful</returns>
    Task<bool> SignUpForCourse(SignUpForCourseDto dto);

    /// <summary>
    /// Creates a new course.
    /// </summary>
    /// <param name="dto">The details of the course to create</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task CreateNewCourse(CreateCourseDto dto);

    /// <summary>
    /// Retrieves the courses created by a specific teacher.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of teacher courses DTOs</returns>
    Task<List<TeacherCoursesDto>> GetMyCreatedCoursesAsync();

    /// <summary>
    /// Retrieves all available courses in the system.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of all available courses</returns>
    Task<List<CourseDto>> GetAllCoursesAsync();

    /// <summary>
    /// Retrieves all courses that the user is enrolled in.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of courses the user is enrolled in</returns>
    Task<List<EnrolledCourseDto>> GetEnrolledCoursesAsync();
}