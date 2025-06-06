namespace BE.Models.Models.Problem;

//TODO: MERGE WITH EXPECTEDOUTPUTLIST ENTITY INTO ONE WHICH WILL CONTAIN THE STDINS AND EXPECTEDOUTPUTS
public class StdInListModel
{
    //TODO change to string since every time i update an entity it auto increments
    public int Id { get; set; }
    public bool IsSample { get; set; }
    public string StdIn { get; set; }
    public string ProblemId { get; set; }
    public ProblemModel Problem { get; set; }
}