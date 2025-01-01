using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace BE.Models.Problem;


[Table("ProblemLanguage")]
public class ProblemLanguageModel
{
    public int ProblemId { get; set; }
    // Doesnt matter if i set these navigation properties to null
    public ProblemModel Problem { get; set; }
    public int LanguageId { get; set; }
    // If I don't JsonIgnore this property i will get it in the response which i dont need, as i only need the ids
    [JsonIgnore]
    // Doesnt matter if i set these navigation properties to null
    public LanguageModel Language { get; set; }
}