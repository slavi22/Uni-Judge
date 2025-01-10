using System.ComponentModel.DataAnnotations.Schema;

namespace BE.Models.Submissions;

public class TestCaseModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; } //token
    public bool IsCorrect { get; set; }
    public string? Stdout { get; set; }
    public string? CompileOutput { get; set; }
    public string? Stderr { get; set; }
    public string? ExpectedOutput { get; set; }
    public string? HiddenExpectedOutput { get; set; }
    public TestCaseStatusModel TestCaseStatus { get; set; }
}

