using BE.DTOs.Problem;
using BE.Models.Problem;

namespace BE.Repositories;

public interface IProblemService
{
    public Task<Problem> CreateProblem(CreateProblemDto dtos);
}