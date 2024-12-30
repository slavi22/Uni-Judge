using Newtonsoft.Json;

namespace BE.Models.Problem;


public class ProblemLanguage
{
    public int ProblemId { get; set; }
    // Doesnt matter if i set these navigation properties to null
    public Problem Problem { get; set; }
    public int LanguageId { get; set; }
    // If I don't JsonIgnore this property i will get it in the response which i dont need, as i only need the ids
    [JsonIgnore]
    // Doesnt matter if i set these navigation properties to null
    public Language Language { get; set; }
}