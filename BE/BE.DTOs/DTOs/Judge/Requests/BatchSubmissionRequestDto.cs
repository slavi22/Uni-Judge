using Newtonsoft.Json;

namespace BE.DTOs.DTOs.Judge.Requests;

public class BatchSubmissionRequestDto
{
    [JsonProperty("submissions")]
    public List<SubmissionRequestDto> Submissions { get; set; } = new List<SubmissionRequestDto>();
}