using Newtonsoft.Json;

namespace BE.DTOs.DTOs.Judge.Responses;

public class SubmissionResultDto
{
    [JsonProperty("submissions")] public List<SubmissionStatusDto> Submissions { get; set; }
}