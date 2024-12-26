using Newtonsoft.Json;

namespace BE.DTOs.Judge.Responses;

public class SubmissionResponseTokenDto
{
    [JsonProperty("token")] public string Token { get; set; }
}

public class SubmissionBatchResultResponse
{
    [JsonProperty("token")] public string Token { get; set; }

    [JsonProperty("status")] public string Status { get; set; }

    [JsonProperty("stderr")] public string? Stderr { get; set; }

    [JsonProperty("expected_output")] public string? ExpectedOutput { get; set; }
}