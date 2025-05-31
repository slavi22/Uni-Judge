namespace BE.DTOs.DTOs.UserSubmission.Responses;

public class TeacherLastUserSubmissionsDto
{
    public string SubmissionId { get; set; }
    public string User { get; set; }
    public bool? IsError { get; set; }
    public string? ErrorResult { get; set; }
    public bool IsPassing { get; set; }
    public string ProblemId { get; set; }
    public string LanguageId { get; set; }
}