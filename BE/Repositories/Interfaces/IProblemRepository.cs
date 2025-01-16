using BE.Models.Problem;

namespace BE.Repositories.Interfaces;

public interface IProblemRepository
{
    Task<ProblemModel> GetProblemByProblemIdAsync(string problemId);
    Task AddProblemAsync(ProblemModel problem);
}