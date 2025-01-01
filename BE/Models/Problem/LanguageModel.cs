using System.ComponentModel.DataAnnotations.Schema;

namespace BE.Models.Problem;

public class LanguageModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<ProblemLanguageModel> ProblemLanguages { get; } = new List<ProblemLanguageModel>();
}