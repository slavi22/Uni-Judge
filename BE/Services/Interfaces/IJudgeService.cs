using BE.DTOs.Judge.Requests;
using BE.DTOs.Judge.Responses;

namespace BE.Services.Interfaces;

public interface IJudgeService
{
    /// <summary>
    /// This method is used to send a batch of submissions to the judge BE and return the status of each submissionModel
    /// </summary>
    /// <param name="clientSubmissionDto">A DTO which contains the submissionModel submitted by the client <see cref="ClientSubmissionDto"/></param>
    /// <returns>A list with the tokens and respective statuses</returns>
    Task<List<SubmissionBatchResultResponseDto>> AddBatchSubmissions(ClientSubmissionDto clientSubmissionDto);
}