using BE.Util.JsonConverters;
using Newtonsoft.Json;

namespace BE.DTOs.Judge.Requests;

// make sure to replace all newline from the frontend with '\n' //code.split('\n') then use loop over it and add \n at the end of each line
public class BatchSubmissionDto
{
    [JsonProperty("submissions")]
    public List<SingleSubmissionDto> Submissions { get; set; }
}

public class SingleSubmissionDto
{
    public int ProblemId { get; set; }

    [JsonProperty("language_id")]
    public string LanguageId { get; set; }

    [JsonProperty("source_code")]
    public string SourceCode { get; set; }

}

public class BatchSubmissionRequestDto
{
    [JsonProperty("submissions")]
    public List<SingleSubmissionRequestDto> Submissions { get; set; } = new List<SingleSubmissionRequestDto>();
}

public class SingleSubmissionRequestDto
{
    [JsonProperty("language_id")]
    public string LanguageId { get; set; }

    [JsonProperty("source_code")]
    [JsonConverter(typeof(Base64Converter))]
    public string SourceCode { get; set; }

    [JsonProperty("stdin")]
    [JsonConverter(typeof(Base64Converter))]
    public string? StdIn { get; set; }

    [JsonProperty("expected_output")]
    [JsonConverter(typeof(Base64Converter))]
    public string? ExpectedOutput { get; set; }
}