using Newtonsoft.Json;

namespace BE.DTOs.Judge;

public class SubmissionStatusDto
{
    [JsonProperty("status")]
    public SubmissionStatusDetailDto Status { get; set; }
}