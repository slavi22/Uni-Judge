namespace BE.DTOs.DTOs.Judge.Requests;

// make sure to replace all newline from the frontend with '\n' //code.split('\n') then use loop over it and add \n at the end of each line

public class ClientSubmissionDto
{
    public string CourseId { get; set; }

    public string ProblemId { get; set; }

    public string LanguageId { get; set; }

    public string SourceCode { get; set; }

}