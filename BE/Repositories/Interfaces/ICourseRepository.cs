using BE.Models.Courses;

namespace BE.Repositories.Interfaces;

public interface ICourseRepository
{
    public Task<bool> SignUpForCourseAsync(CoursesModel course, UserCourseModel userCourse);
    public Task CreateCourseAsync(CoursesModel courseModel);
    public Task<CoursesModel> GetCourseByIdAsync(string id);
    public Task<CoursesModel> GetCourseAndProblemsByIdAsync(string id);
}