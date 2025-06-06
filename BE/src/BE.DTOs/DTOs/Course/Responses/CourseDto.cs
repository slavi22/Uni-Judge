namespace BE.DTOs.DTOs.Course.Responses;

public class CourseDto
{
    public string CourseId { get; set; }
    public string Name { get; set; }
    public bool IsPasswordProtected { get; set; }
    public bool UserIsEnrolled { get; set; }
}