using BE.DTOs.DTOs.Problem.Requests;
using BE.DTOs.DTOs.Problem.Responses;

namespace BE.Business.Services.Interfaces;

public interface IProblemService
{
    /// <summary>
    /// Creates a new problem based on the provided client problem DTO.
    /// </summary>
    /// <param name="dto">The client problem DTO containing the problem details</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created problem DTO</returns>
    public Task<CreatedProblemDto> CreateProblem(ClientProblemDto dto);
}