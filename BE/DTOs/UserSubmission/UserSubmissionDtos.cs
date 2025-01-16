namespace BE.DTOs.UserSubmission;

public class UserSubmissionDto
{
    public string Token { get; set; }
    public List<TestCaseDto> TestCases { get; set; } = new List<TestCaseDto>();
}

public class TestCaseDto
{
    public bool IsCorrect { get; set; }
    public string? ExpectedOutput { get; set; }
    public string? CompileOutput { get; set; }
    public string? Stdout { get; set; }
    public TestCaseStatusDto Status { get; set; }
}

public class TestCaseStatusDto
{
    public int Id { get; set; }
    public string Description { get; set; }
}