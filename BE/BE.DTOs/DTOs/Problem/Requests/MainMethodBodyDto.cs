using BE.Models.Models.Problem.Enums;

namespace BE.DTOs.DTOs.Problem.Requests;

public class MainMethodBodyDto
{
    public LanguagesEnum LanguageId { get; set; }
    public string SolutionTemplate { get; set; }
    public string MainMethodBodyContent { get; set; }
}