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
    Task<CreatedProblemDto> CreateProblemAsync(ClientProblemDto dto);

    /// <summary>
    /// Retrieves the problems created by a specific teacher.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the teacher problems DTO with all problems created by the teacher</returns>
    Task<List<TeacherProblemsDto>> GeyMyProblemsAsync();
}