using BE.Models.Models.Submissions;

namespace BE.DataAccess.Repositories.Interfaces;

public interface IUserSubmissionRepository
{
    /// <summary>
    /// Adds a new user submission to the repository.
    /// </summary>
    /// <param name="submissionModel">The user submission model to add</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task AddAsync(UserSubmissionModel submissionModel);

    /// <summary>
    /// Retrieves all submissions made by a user for a specific problem in a course.
    /// </summary>
    /// <param name="courseId">The unique identifier of the course</param>
    /// <param name="problemId">The unique identifier of the problem</param>
    /// <param name="userId">The unique identifier of the user</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of the user's submissions for the specified problem</returns>
    Task<List<UserSubmissionModel>> GetAllUserSubmissionsForSpecificProblem(string courseId, string problemId, string userId);
}