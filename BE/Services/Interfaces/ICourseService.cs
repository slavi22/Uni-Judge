using BE.DTOs.Course;
using BE.DTOs.Course.Requests;

namespace BE.Services.Interfaces;

public interface ICourseService
{
    Task<List<ViewCourseProblemDto>> GetProblemsForCourse(string courseId);
    Task<bool> SignUpForCourse(SignUpForCourseDto dto);
    Task CreateNewCourse(CreateCourseDto dto);
}