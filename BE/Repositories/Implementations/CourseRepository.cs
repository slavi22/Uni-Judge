using BE.Data;
using BE.Models.Courses;
using BE.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories.Implementations;

public class CourseRepository : ICourseRepository
{
    private readonly AppDbContext _dbContext;

    public CourseRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> SignUpForCourseAsync(CoursesModel course, UserCourseModel userCourse)
    {
        course.UserCourses.Add(userCourse);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task CreateCourseAsync(CoursesModel courseModel)
    {
        await _dbContext.Courses.AddAsync(courseModel);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<CoursesModel> GetCourseByIdAsync(string id)
    {
        return await _dbContext.Courses.FindAsync(id);
    }

    public async Task<CoursesModel> GetCourseAndProblemsByIdAsync(string id)
    {
        return await _dbContext.Courses.Include(c => c.Problems).FirstOrDefaultAsync(c => c.Id == id);
    }
}