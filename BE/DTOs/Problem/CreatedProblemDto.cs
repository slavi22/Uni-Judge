namespace BE.DTOs.Problem;

public class CreatedProblemDto
{
    public string ProblemId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int RequiredPercentageToPass { get; set; }
    public string CourseId { get; set; }
}