using BE.Models.Models.Problem.Enums;

namespace BE.Models.Models.Problem;

public class MainMethodBodyModel
{
    //TODO change to string since every time i update an entity it auto increments
    public int Id { get; set; }
    public LanguagesEnum LanguageId { get; set; }
    public string SolutionTemplate { get; set; }
    public string MainMethodBodyContent { get; set; }
    public string ProblemId { get; set; }
    public ProblemModel Problem { get; set; }
}