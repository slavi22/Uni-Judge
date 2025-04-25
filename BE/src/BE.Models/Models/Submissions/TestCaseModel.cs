using System.ComponentModel.DataAnnotations.Schema;

namespace BE.Models.Models.Submissions;

public class TestCaseModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string Id { get; set; } //token
    public bool IsCorrect { get; set; }
    public string? Stdout { get; set; }
    public string? CompileOutput { get; set; }
    public string? Stderr { get; set; }
    public string? ExpectedOutput { get; set; }
    public string? HiddenExpectedOutput { get; set; }
    public string UserSubmissionId { get; set; }
    public UserSubmissionModel UserSubmission { get; set; }
    public TestCaseStatusModel TestCaseStatus { get; set; }
}

