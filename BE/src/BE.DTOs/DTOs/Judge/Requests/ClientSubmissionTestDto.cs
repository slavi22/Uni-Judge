namespace BE.DTOs.DTOs.Judge.Requests;

public class ClientSubmissionTestDto
{
    public string CourseId { get; set; }
    public string ProblemId { get; set; }
    public string LanguageId { get; set; }
    public string SourceCode { get; set; }
    public List<ClientTestStdInsAndExpectedOutput> TestCases { get; set; }
}