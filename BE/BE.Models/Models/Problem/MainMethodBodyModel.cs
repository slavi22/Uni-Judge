namespace BE.Models.Models.Problem;

public class MainMethodBodyModel
{
    public int Id { get; set; }
    public LanguagesEnum Language { get; set; }
    public string SolutionTemplate { get; set; }
    public string MainMethodBodyContent { get; set; }
    public string ProblemId { get; set; }
    public ProblemModel Problem { get; set; }
}

public enum LanguagesEnum
{
    // we will have a select with the value set as the enum index and between the option tags we will have the enum name

    // C# language id,
    CSharp = 51
}