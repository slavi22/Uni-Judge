﻿using BE.Common.Util.JsonConverters;
using Newtonsoft.Json;

namespace BE.DTOs.DTOs.Judge.Responses;

public class SubmissionStatusDto
{
    [JsonProperty("status")] public StatusDto Status { get; set; }

    [JsonProperty("stdout")]
    [JsonConverter(typeof(PlainTextConverter))]
    public string Stdout { get; set; }

    [JsonProperty("compile_output")]
    [JsonConverter(typeof(PlainTextConverter))]
    public string? CompileOutput { get; set; }

    [JsonProperty("stderr")]
    [JsonConverter(typeof(PlainTextConverter))]
    public string Stderr { get; set; }

    [JsonProperty("expected_output")] public string ExpectedOutput { get; set; }
}