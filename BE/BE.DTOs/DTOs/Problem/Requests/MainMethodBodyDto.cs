namespace BE.DTOs.DTOs.Problem.Requests;

public class MainMethodBodyDto
{
    public LanguagesEnumDto Language { get; set; }
    public string SolutionTemplate { get; set; }
    public string MainMethodBodyContent { get; set; }
}