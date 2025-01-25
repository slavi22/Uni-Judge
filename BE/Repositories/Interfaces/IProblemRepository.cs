using BE.Models.Problem;

namespace BE.Repositories.Interfaces;

public interface IProblemRepository
{
    /// <summary>
    /// Retrieves a problem by its ID.
    /// </summary>
    /// <param name="problemId">The ID of the problem to retrieve</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the problem model</returns>
    Task<ProblemModel> GetProblemByProblemIdAsync(string problemId);

    /// <summary>
    /// Adds a new problem to the repository.
    /// </summary>
    /// <param name="problem">The problem model to add</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task AddProblemAsync(ProblemModel problem);
}