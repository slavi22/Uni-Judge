using BE.Models.Courses;

namespace BE.Repositories.Interfaces;

public interface ICourseRepository
{
    public Task CreateCourseAsync(CoursesModel courseModel);
    public Task<CoursesModel> GetCourseByIdAsync(string id);
}