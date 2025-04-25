using Newtonsoft.Json;

namespace BE.DTOs.DTOs.Judge.Responses;

public class StatusDto
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("description")] public string Description { get; set; }
}