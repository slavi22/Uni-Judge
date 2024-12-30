using BE.DTOs.Problem;

namespace BE.Models.Problem;

public class MainMethodBody
{
    public int Id { get; set; }
    public LanguagesEnum Language { get; set; }
    public string MainMethodBodyContent { get; set; }
    public Problem Problem { get; set; }
    public int ProblemId { get; set; }
}