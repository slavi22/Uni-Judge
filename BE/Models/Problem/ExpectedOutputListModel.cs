namespace BE.Models.Problem;

public class ExpectedOutputListModel
{
    public int Id { get; set; }
    public string ExpectedOutput { get; set; }
    public int ProblemId { get; set; }
    public ProblemModel Problem { get; set; }
}