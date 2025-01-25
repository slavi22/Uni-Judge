using BE.Models.Courses;

namespace BE.Repositories.Interfaces;

public interface ICourseRepository
{
    /// <summary>
    /// Signs up a user for a course
    /// </summary>
    /// <param name="course">The course model which contains the "UserCourse" collection, which will be update</param>
    /// <param name="userCourse">The new entity we add in the "UserCourse" collection</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the sign-up was successful</returns>
    public Task<bool> SignUpForCourseAsync(CoursesModel course, UserCourseModel userCourse);

    /// <summary>
    /// Creates a new course and saves it to the database.
    /// </summary>
    /// <param name="courseModel">The course model to be created</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public Task CreateCourseAsync(CoursesModel courseModel);

    /// <summary>
    /// Retrieves a course by its ID.
    /// </summary>
    /// <param name="id">The ID of the course to retrieve</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the course model</returns>
    public Task<CoursesModel> GetCourseByIdAsync(string id);

    /// <summary>
    /// Retrieves a course by its ID, including its associated problems.
    /// </summary>
    /// <param name="id">The ID of the course to retrieve</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the course model with its associated problems</returns>
    public Task<CoursesModel> GetCourseAndProblemsByIdAsync(string id);
}