using BE.DTOs.DTOs.Judge.Requests;
using BE.DTOs.DTOs.Judge.Responses;

namespace BE.Business.Services.Interfaces;

public interface IJudgeService
{
    /// <summary>
    /// This method is used to send a batch of submissions to the judge BE and return the status of each submissionModel
    /// </summary>
    /// <param name="clientSubmissionDto">A DTO which contains the submissionModel submitted by the client <see cref="ClientSubmissionDto"/></param>
    /// <returns>A list with the tokens and respective statuses</returns>
    Task<List<SubmissionBatchResultResponseDto>> CreateBatchSubmissions(ClientSubmissionDto clientSubmissionDto);

    /// <summary>
    /// This method is used to test a batch of submissions against the judge without saving the results to the database
    /// </summary>
    /// <param name="clientSubmissionTestDto">A DTO which contains the submission submitted by the client for testing purposes</param>
    /// <returns>A list of submission results with status information for each test case</returns>
    Task<List<TestSubmissionBatchResultResponseDto>> TestBatchSubmissions(ClientSubmissionTestDto clientSubmissionTestDto);
}