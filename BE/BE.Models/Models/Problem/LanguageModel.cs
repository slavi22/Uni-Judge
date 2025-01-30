using System.ComponentModel.DataAnnotations.Schema;
using BE.Models.Models.Submissions;

namespace BE.Models.Models.Problem;

public class LanguageModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<UserSubmissionModel> UserSubmissions { get; set; } = new List<UserSubmissionModel>();
    public ICollection<ProblemLanguageModel> ProblemLanguages { get; } = new List<ProblemLanguageModel>();
}