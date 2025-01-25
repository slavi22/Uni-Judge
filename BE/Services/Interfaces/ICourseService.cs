using BE.DTOs.Course;
using BE.DTOs.Course.Requests;

namespace BE.Services.Interfaces;

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
}