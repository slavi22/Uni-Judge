using Newtonsoft.Json;

namespace BE.DTOs.Judge.Responses;

public class SubmissionResponseTokenDto
{
    [JsonProperty("token")] public string Token { get; set; }
}

public class SubmissionBatchResultResponse
{
    public bool IsCorrect { get; set; }

    [JsonProperty("token")] public string Token { get; set; }

    [JsonProperty("stdout")] public string Stdout { get; set; }

    [JsonProperty("status")] public StatusDto Status { get; set; }

    [JsonProperty("compile_output")] public string? CompileOutput { get; set; }

    [JsonProperty("stderr")] public string? Stderr { get; set; }

    [JsonProperty("expected_output")] public string? ExpectedOutput { get; set; }
}