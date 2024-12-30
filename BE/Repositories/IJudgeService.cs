using BE.DTOs.Judge.Requests;
using BE.DTOs.Judge.Responses;

namespace BE.Repositories;

public interface IJudgeService
{
    /// <summary>
    /// This method is used to send a batch of submissions to the judge BE and return the status of each submission
    /// </summary>
    /// <param name="batchSubmissions">A DTO which contains a list of submissions <see cref="SubmissionDto"/></param>
    /// <returns>A list with the tokens and respective statuses</returns>
    Task<List<SubmissionBatchResultResponse>> AddBatchSubmissions(BatchSubmissionDto batchSubmissions);
}