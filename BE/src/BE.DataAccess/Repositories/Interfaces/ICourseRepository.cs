using BE.Models.Models.Courses;

namespace BE.DataAccess.Repositories.Interfaces;

public interface ICourseRepository
{
    /// <summary>
    /// Signs up a user for a course
    /// </summary>
    /// <param name="course">The course model which contains the "UserCourse" collection, which will be update</param>
    /// <param name="userCourse">The new entity we add in the "UserCourse" collection</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the sign-up was successful</returns>
    Task<bool> SignUpForCourseAsync(CoursesModel course, UserCourseModel userCourse);

    /// <summary>
    /// Creates a new course and saves it to the database.
    /// </summary>
    /// <param name="courseModel">The course model to be created</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task CreateCourseAsync(CoursesModel courseModel);

    /// <summary>
    /// Retrieves a course by its ID.
    /// </summary>
    /// <param name="courseId">The ID of the course to retrieve</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the course model</returns>
    Task<CoursesModel> GetCourseByCourseIdAsync(string courseId);

    /// <summary>
    /// Retrieves a course by its ID, including its associated problems.
    /// </summary>
    /// <param name="courseId">The ID of the course to retrieve</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the course model with its associated problems</returns>
    Task<CoursesModel> GetCourseAndProblemsByIdAsync(string courseId);

    /// <summary>
    /// Retrieves all courses created by a specific teacher.
    /// </summary>
    /// <param name="teacherId">The ID of the teacher whose courses to retrieve</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of course models created by the teacher</returns>
    Task<List<CoursesModel>> GetTeacherCoursesAsync(string teacherId);

    /// <summary>
    /// Retrieves all courses available in the system.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of all course models</returns>
    Task<List<CoursesModel>> GetAllCoursesAsync();

}