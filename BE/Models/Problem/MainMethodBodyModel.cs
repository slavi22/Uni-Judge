using BE.DTOs.Problem;

namespace BE.Models.Problem;

public class MainMethodBodyModel
{
    public int Id { get; set; }
    public LanguagesEnum Language { get; set; }
    public string SolutionTemplate { get; set; }
    public string MainMethodBodyContent { get; set; }
    public Guid ProblemId { get; set; }
    public ProblemModel Problem { get; set; }
}