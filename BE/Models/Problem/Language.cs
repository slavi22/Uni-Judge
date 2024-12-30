using System.ComponentModel.DataAnnotations.Schema;

namespace BE.Models.Problem;

public class Language
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<ProblemLanguage> ProblemLanguages { get; } = new List<ProblemLanguage>();
}