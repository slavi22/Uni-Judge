namespace BE.DTOs.DTOs.Judge.Responses;

public class TestSubmissionBatchResultResponseDto
{
    public bool IsCorrect { get; set; }
    public string? ExpectedOutput { get; set; }
    public string? StdIn { get; set; }
    public string? Stdout { get; set; }
    public StatusDto Status { get; set; }
    public string? CompileOutput { get; set; }
    public string? Stderr { get; set; }
}