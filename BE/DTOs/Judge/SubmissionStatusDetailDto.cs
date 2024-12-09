using Newtonsoft.Json;

namespace BE.DTOs.Judge;

public class SubmissionStatusDetailDto
{
    [JsonProperty("id")]
    public int Id { get; set; }
    [JsonProperty("description")]
    public string Description { get; set; }
}