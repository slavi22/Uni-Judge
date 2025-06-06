using BE.DataAccess.Data;
using BE.DataAccess.Repositories.Interfaces;
using BE.Models.Models.Courses;
using Microsoft.EntityFrameworkCore;

namespace BE.DataAccess.Repositories.Implementations;

public class CourseRepository : ICourseRepository
{
    private readonly AppDbContext _dbContext;

    public CourseRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<bool> SignUpForCourseAsync(CoursesModel course, UserCourseModel userCourse)
    {
        // AsNoTracking() is used to avoid tracking the entity  because we only use it for fetching
        var fetchedCourse =
            await _dbContext.Courses.Include(x => x.UserCourses).AsNoTracking()
                .FirstOrDefaultAsync(x => x.CourseId == course.CourseId);
        // check if the user is already signed up for the course by checking if the user id and course id are already in the UserCourses list
        if (fetchedCourse.UserCourses.Any(x => x.UserId == userCourse.UserId && x.CourseId == userCourse.CourseId) ==
            false)
        {
            course.UserCourses.Add(userCourse);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task CreateCourseAsync(CoursesModel courseModel)
    {
        await _dbContext.Courses.AddAsync(courseModel);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<CoursesModel> GetCourseByCourseIdAsync(string courseId)
    {
        return await _dbContext.Courses.FirstOrDefaultAsync(x => x.CourseId == courseId);
    }

    public async Task<CoursesModel> GetCourseAndProblemsByIdAsync(string courseId)
    {
        return await _dbContext.Courses.Include(c => c.Problems).FirstOrDefaultAsync(c => c.CourseId == courseId);
    }

    public async Task<List<CoursesModel>> GetTeacherCoursesAsync(string teacherId)
    {
        var teacherCourses = await _dbContext.Courses
                .Include(c => c.User)
                .Where(c => c.UserId == teacherId).ToListAsync();
        return teacherCourses;
    }

    public async Task<List<CoursesModel>> GetAllCoursesAsync()
    {
        var allCourses = await _dbContext.Courses
            .Include(c => c.UserCourses).ToListAsync();
        return allCourses;
    }

    public async Task<List<CoursesModel>> GetAllEnrolledCoursesAsync(string userId)
    {
        var enrolledCourses = await _dbContext.Courses
            .Include(c => c.UserCourses)
            .Where(c => c.UserCourses.Any(uc => uc.UserId == userId))
            .ToListAsync();
        return enrolledCourses;
    }

    public async Task<int> DeleteCourseByCourseId(CoursesModel course)
    {
       _dbContext.Remove(course);
       var count = await _dbContext.SaveChangesAsync();
       return count;
    }
}