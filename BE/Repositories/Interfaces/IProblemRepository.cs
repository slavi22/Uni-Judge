using BE.Models.Problem;

namespace BE.Repositories.Interfaces;

public interface IProblemRepository
{
    Task<ProblemModel> GetProblemByIdAsync(int id);
    Task AddProblemAsync(ProblemModel problem);
}