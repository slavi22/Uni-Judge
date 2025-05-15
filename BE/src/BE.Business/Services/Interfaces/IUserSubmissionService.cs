using BE.DTOs.DTOs.Judge.Requests;
using BE.DTOs.DTOs.Judge.Responses;
using BE.DTOs.DTOs.UserSubmission.Responses;

namespace BE.Business.Services.Interfaces;

public interface IUserSubmissionService
{
    /// <summary>
    /// Adds a new user submission based on the provided client submission DTO and batch result response DTOs.
    /// </summary>
    /// <param name="clientSubmissionDto">The client submission DTO containing the submission details</param>
    /// <param name="submissionBatchResultResponseDto">The list of submission batch result response DTOs</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created user submission DTO</returns>
    Task<UserSubmissionResultDto> AddUserSubmission(ClientSubmissionDto clientSubmissionDto, List<SubmissionBatchResultResponseDto> submissionBatchResultResponseDto);


    /// <summary>
    /// Retrieves all submissions made by a user for a specific problem in a course.
    /// </summary>
    /// <param name="courseId">The unique identifier of the course</param>
    /// <param name="problemId">The unique identifier of the problem</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of the user's submissions for the specified problem</returns>
    Task<List<ProblemUserSubmissionsDto>> GetUserSubmissionsForSpecificProblem(string courseId, string problemId);
}