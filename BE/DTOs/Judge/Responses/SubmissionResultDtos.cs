using Newtonsoft.Json;

namespace BE.DTOs.Judge.Responses;

public class SubmissionResultDto
{
    [JsonProperty("submissions")] public List<SubmissionStatusDto> Submissions { get; set; }
}

public class SubmissionStatusDto
{
    [JsonProperty("status")] public StatusDto Status { get; set; }

    [JsonProperty("stderr")] public string Stderr { get; set; }

    [JsonProperty("expected_output")] public string ExpectedOutput { get; set; }
}

public class StatusDto
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("description")] public string Description { get; set; }
}