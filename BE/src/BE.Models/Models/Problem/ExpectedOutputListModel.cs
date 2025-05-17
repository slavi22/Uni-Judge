namespace BE.Models.Models.Problem;

//TODO: MERGE WITH STDINLIST ENTITY INTO ONE WHICH WILL CONTAIN THE STDINS AND EXPECTEDOUTPUTS
public class ExpectedOutputListModel
{
    //TODO change to string since every time i update an entity it auto increments
    public int Id { get; set; }
    public bool IsSample { get; set; }
    public string ExpectedOutput { get; set; }
    public string ProblemId { get; set; }
    public ProblemModel Problem { get; set; }
}