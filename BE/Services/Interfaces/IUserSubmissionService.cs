using BE.DTOs.Judge.Requests;
using BE.DTOs.Judge.Responses;

namespace BE.Services.Interfaces;

public interface IUserSubmissionService
{
    public Task AddUserSubmission(ClientSubmissionDto clientSubmissionDto, List<SubmissionBatchResultResponseDto> submissionBatchResultResponseDto);
}