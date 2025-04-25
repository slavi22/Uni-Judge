using System.ComponentModel.DataAnnotations;
using BE.Models.Models.Problem.Enums;

namespace BE.DTOs.DTOs.Problem.Requests;

public class ClientProblemDto
{
    public string ProblemId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    [Range(0, 100)]
    public int RequiredPercentageToPass { get; set; }
    public string CourseId { get; set; }
    public List<MainMethodBodyDto> MainMethodBodiesList { get; set; }
    public List<ExpectedOutputListDto> ExpectedOutputList { get; set; }
    public List<string> StdInList { get; set; }
    public List<LanguagesEnum> LanguagesList { get; set; }
}