using Newtonsoft.Json;

namespace BE.DTOs.Judge.Responses;

public class SubmissionResponseTokenDto
{
    [JsonProperty("token")] public string Token { get; set; }
}

public class SubmissionBatchResultResponseDto
{
    public bool IsCorrect { get; set; }

    public string? HiddenExpectedOutput { get; set; }

    public string Token { get; set; }

    public string? Stdout { get; set; }

    public StatusDto Status { get; set; }

    public string? CompileOutput { get; set; }

    public string? Stderr { get; set; }

    public string? ExpectedOutput { get; set; }
}