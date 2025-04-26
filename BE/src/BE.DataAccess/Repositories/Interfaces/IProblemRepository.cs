using BE.Models.Models.Problem;

namespace BE.DataAccess.Repositories.Interfaces;

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

    /// <summary>
    /// Retrieves all problems associated with a specific teacher.
    /// </summary>
    /// <param name="teacherId">The unique identifier of the teacher whose problems are to be retrieved</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of problem models created by the teacher</returns>
    Task<List<ProblemModel>> GetTeacherProblems(string teacherId);
}