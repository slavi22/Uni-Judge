using BE.DTOs.Problem;
using BE.Models.Problem;

namespace BE.Services.Interfaces;

public interface IProblemService
{
    public Task<ProblemModel> CreateProblem(CreateProblemDto dto);
}