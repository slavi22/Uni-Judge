using Newtonsoft.Json;

namespace BE.DTOs.Judge;

public class SubmissionBatchDto
{
    [JsonProperty("submissions")]
    public List<SubmissionDto> Submissions { get; set; }
}