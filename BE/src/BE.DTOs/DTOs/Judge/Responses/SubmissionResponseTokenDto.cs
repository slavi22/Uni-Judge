using Newtonsoft.Json;

namespace BE.DTOs.DTOs.Judge.Responses;

public class SubmissionResponseTokenDto
{
    [JsonProperty("token")] public string Token { get; set; }
}