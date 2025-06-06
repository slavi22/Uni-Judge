using System.ComponentModel.DataAnnotations;
using BE.DTOs.DTOs.Problem.Requests;
using BE.Models.Models.Problem.Enums;

namespace BE.DTOs.DTOs.Problem.Responses;

public class EditProblemInfoDto
{
    public string CourseId { get; set; }
    public string ProblemId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    [Range(0, 100)]
    public int RequiredPercentageToPass { get; set; }
    [MinLength(1, ErrorMessage = "At least one main method body must be provided.")]
    public List<MainMethodBodyDto> MainMethodBodiesList { get; set; }
    [MinLength(1, ErrorMessage = "At least one expected output must be provided.")]
    public List<ExpectedOutputListDto> ExpectedOutputList { get; set; }
    [MinLength(1, ErrorMessage = "At least one test case must be provided.")]
    public List<StdInListDto> StdInList { get; set; }
    [MinLength(1, ErrorMessage = "At least one language must be selected.")]
    public List<LanguagesEnum> LanguagesList { get; set; }
}