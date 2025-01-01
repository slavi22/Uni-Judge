using BE.DTOs.Problem;
using BE.Models.Problem;

namespace BE.Repositories;

public interface IProblemService
{
    public Task<ProblemModel> CreateProblem(CreateProblemDto dto);
}