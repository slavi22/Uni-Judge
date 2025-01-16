namespace BE.Models.Problem;

public class StdInListModel
{
    public int Id { get; set; }
    public string StdIn { get; set; }
    public Guid ProblemId { get; set; }
    public ProblemModel Problem { get; set; }
}