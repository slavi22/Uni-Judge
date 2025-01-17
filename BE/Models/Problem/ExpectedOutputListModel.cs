namespace BE.Models.Problem;

public class ExpectedOutputListModel
{
    public int Id { get; set; }
    public bool IsSample { get; set; }
    public string ExpectedOutput { get; set; }
    public string ProblemId { get; set; }
    public ProblemModel Problem { get; set; }
}