using BE.Models.Auth;
using BE.Models.Problem;

namespace BE.Models.Submissions;

public class UserSubmissionModel
{
    public Guid Id { get; set; }
    public string SourceCode { get; set; }
    public bool IsPassing { get; set; }

    public string UserId { get; set; }
    public AppUser User { get; set; }
    public int ProblemId { get; set; }
    public ProblemModel Problem { get; set; }
    public int LanguageId { get; set; }
    public LanguageModel Language { get; set; }

    public ICollection<TestCaseModel> TestCases { get; set; } = new List<TestCaseModel>();
}