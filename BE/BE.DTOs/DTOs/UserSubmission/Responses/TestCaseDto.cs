namespace BE.DTOs.DTOs.UserSubmission.Responses;

public class TestCaseDto
{
    public bool IsCorrect { get; set; }
    public string? ExpectedOutput { get; set; }
    public string? CompileOutput { get; set; }
    public string? Stdout { get; set; }
    public TestCaseStatusDto Status { get; set; }
}