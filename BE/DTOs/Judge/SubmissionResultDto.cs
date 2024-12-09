using Newtonsoft.Json;

namespace BE.DTOs.Judge;

public class SubmissionResultDto
{
    [JsonProperty("submissions")]
    public List<SubmissionStatusDto> Submissions { get; set; }
}