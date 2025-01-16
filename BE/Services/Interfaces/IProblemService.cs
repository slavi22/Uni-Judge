using BE.DTOs.Problem;
using BE.Models.Problem;

namespace BE.Services.Interfaces;

public interface IProblemService
{
    public Task<CreatedProblemDto> CreateProblem(ClientProblemDto dto);
}