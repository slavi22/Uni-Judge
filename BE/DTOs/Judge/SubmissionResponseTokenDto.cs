using Newtonsoft.Json;

namespace BE.DTOs.Judge;

public class SubmissionResponseTokenDto
{
    [JsonProperty("token")]
    public string Token { get; set; }
}