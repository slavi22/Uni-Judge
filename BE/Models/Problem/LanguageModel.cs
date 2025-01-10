using System.ComponentModel.DataAnnotations.Schema;
using BE.Models.Submissions;

namespace BE.Models.Problem;

public class LanguageModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public string Name { get; set; }
    public UserSubmissionModel UserSubmissionModel { get; set; }
    public ICollection<ProblemLanguageModel> ProblemLanguages { get; } = new List<ProblemLanguageModel>();
}