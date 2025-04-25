using BE.Common.Util.JsonConverters;
using Newtonsoft.Json;

namespace BE.DTOs.DTOs.Judge.Requests;

public class SubmissionRequestDto
{
    [JsonProperty("language_id")] public string LanguageId { get; set; }

    [JsonProperty("source_code")]
    [JsonConverter(typeof(Base64Converter))]
    public string SourceCode { get; set; }

    [JsonProperty("stdin")]
    [JsonConverter(typeof(Base64Converter))]
    public string? StdIn { get; set; }

    [JsonProperty("expected_output")]
    [JsonConverter(typeof(Base64Converter))]
    public string? ExpectedOutput { get; set; }

    public string? HiddenExpectedOutput { get; set; }
}