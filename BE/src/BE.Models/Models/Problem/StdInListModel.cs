namespace BE.Models.Models.Problem;

public class StdInListModel
{
    public int Id { get; set; }
    public bool IsSample { get; set; }
    public string StdIn { get; set; }
    public string ProblemId { get; set; }
    public ProblemModel Problem { get; set; }
}