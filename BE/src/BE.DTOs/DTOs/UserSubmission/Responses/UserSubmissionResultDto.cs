namespace BE.DTOs.DTOs.UserSubmission.Responses;

public class UserSubmissionResultDto
{
    public string SumbissionId { get; set; }
    public bool IsError { get; set; }
    public List<TestCaseDto> TestCases { get; set; } = new List<TestCaseDto>();
}