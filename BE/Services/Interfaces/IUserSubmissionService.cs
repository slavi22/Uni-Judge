using BE.DTOs.Judge.Requests;
using BE.DTOs.Judge.Responses;
using BE.DTOs.UserSubmission;

namespace BE.Services.Interfaces;

public interface IUserSubmissionService
{
    public Task<UserSubmissionDto> AddUserSubmission(ClientSubmissionDto clientSubmissionDto, List<SubmissionBatchResultResponseDto> submissionBatchResultResponseDto);
}