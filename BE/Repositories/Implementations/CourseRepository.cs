using BE.Data;
using BE.Models.Courses;
using BE.Repositories.Interfaces;

namespace BE.Repositories.Implementations;

public class CourseRepository : ICourseRepository
{
    private readonly AppDbContext _dbContext;

    public CourseRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateCourseAsync(CoursesModel courseModel)
    {
        await _dbContext.Courses.AddAsync(courseModel);
        await _dbContext.SaveChangesAsync();
    }
}