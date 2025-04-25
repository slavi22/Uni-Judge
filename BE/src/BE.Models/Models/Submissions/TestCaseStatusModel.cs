namespace BE.Models.Models.Submissions;

public class TestCaseStatusModel
{
    public int Id { get; set; }
    public int ResultId { get; set; }
    public string Description { get; set; }
    public string TestCaseId { get; set; }
    public TestCaseModel TestCase { get; set; }
}