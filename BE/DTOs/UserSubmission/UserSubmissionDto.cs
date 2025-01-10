namespace BE.DTOs.UserSubmission;

public class UserSubmissionDto
{
    public string Token { get; set; }
    public string CompileOutput { get; set; }
    public string? ExecutionOutput { get; set; }
    //public List<> //list of testcases
}