using BE.Models.Submissions;

namespace BE.Repositories.Interfaces;

public interface IUserSubmissionRepository
{
    /// <summary>
    /// Adds a new user submission to the repository.
    /// </summary>
    /// <param name="submissionModel">The user submission model to add</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public Task AddAsync(UserSubmissionModel submissionModel);
}