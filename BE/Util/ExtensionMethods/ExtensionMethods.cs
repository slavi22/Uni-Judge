using BE.DTOs.Judge.Requests;
using Newtonsoft.Json;

namespace BE.Util.ExtensionMethods;

public static class ExtensionMethods
{
    // https://www.wwt.com/article/how-to-clone-objects-in-dotnet-core
    public static ClientSubmissionDto DeepCopyClientSubmissionDto(this ClientSubmissionDto self)
    {
        var serialized = JsonConvert.SerializeObject(self);
        return JsonConvert.DeserializeObject<ClientSubmissionDto>(serialized);
    }
}