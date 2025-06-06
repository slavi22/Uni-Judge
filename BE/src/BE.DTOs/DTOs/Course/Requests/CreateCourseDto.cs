namespace BE.DTOs.DTOs.Course.Requests;

public class CreateCourseDto
{
    public string CourseId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string? Password { get; set; }
}