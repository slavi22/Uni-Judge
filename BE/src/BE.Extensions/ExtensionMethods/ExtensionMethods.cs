using BE.DTOs.DTOs.Judge.Requests;
using Newtonsoft.Json;

namespace BE.Extensions.ExtensionMethods;

public static class ExtensionMethods
{
    // https://www.wwt.com/article/how-to-clone-objects-in-dotnet-core
    public static ClientSubmissionDto DeepCopyClientSubmissionDto(this ClientSubmissionDto self)
    {
        var serialized = JsonConvert.SerializeObject(self);
        return JsonConvert.DeserializeObject<ClientSubmissionDto>(serialized);
    }

    public static ClientSubmissionTestDto DeepCopyClientSubmissionTestDto(this ClientSubmissionTestDto self)
    {
        var serialized = JsonConvert.SerializeObject(self);
        return JsonConvert.DeserializeObject<ClientSubmissionTestDto>(serialized);
    }
}