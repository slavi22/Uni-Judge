namespace BE.DTOs.DTOs.UserSubmission.Responses;

public class UserSubmissionDto
{
    public string SumbissionId { get; set; }
    public List<TestCaseDto> TestCases { get; set; } = new List<TestCaseDto>();
}