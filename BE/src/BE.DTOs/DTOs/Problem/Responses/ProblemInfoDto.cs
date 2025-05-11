using BE.Models.Models.Problem.Enums;

namespace BE.DTOs.DTOs.Problem.Responses;

public class ProblemInfoDto
{
    public string CourseId { get; set; }
    public string ProblemId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> SolutionTemplate { get; set; }
    public List<string> ExpectedOutputList { get; set; }
    public List<string> StdInList { get; set; }
    public List<LanguagesEnum> AvailableLanguages { get; set; }
}