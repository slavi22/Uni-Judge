using BE.DTOs.Judge.Requests;
using BE.DTOs.Judge.Responses;
using BE.DTOs.UserSubmission;

namespace BE.Services.Interfaces;

public interface IUserSubmissionService
{
    /// <summary>
    /// Adds a new user submission based on the provided client submission DTO and batch result response DTOs.
    /// </summary>
    /// <param name="clientSubmissionDto">The client submission DTO containing the submission details</param>
    /// <param name="submissionBatchResultResponseDto">The list of submission batch result response DTOs</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created user submission DTO</returns>
    public Task<UserSubmissionDto> AddUserSubmission(ClientSubmissionDto clientSubmissionDto, List<SubmissionBatchResultResponseDto> submissionBatchResultResponseDto);
}