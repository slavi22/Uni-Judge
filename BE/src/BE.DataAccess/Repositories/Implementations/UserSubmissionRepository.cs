using BE.DataAccess.Data;
using BE.DataAccess.Repositories.Interfaces;
using BE.Models.Models.Submissions;
using Microsoft.EntityFrameworkCore;

namespace BE.DataAccess.Repositories.Implementations;

public class UserSubmissionRepository : IUserSubmissionRepository
{
    private readonly AppDbContext _dbContext;

    public UserSubmissionRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(UserSubmissionModel submissionModel)
    {
        await _dbContext.UserSubmissions.AddAsync(submissionModel);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<UserSubmissionModel>> GetAllUserSubmissionsForSpecificProblemAsync(string courseId,
        string problemId, string userId)
    {
        var userSubmissions = await _dbContext.UserSubmissions
            .Include(us => us.Course)
            .Include(us => us.Problem)
            .Include(us => us.TestCases)
            .ThenInclude(tc => tc.TestCaseStatus)
            .Where(us => us.UserId == userId && us.Course.CourseId == courseId && us.Problem.ProblemId == problemId)
            .ToListAsync();
        return userSubmissions;
    }

    public async Task<List<UserSubmissionModel>> GetLastUserSubmissionsForProblemAsync(string courseId,
        string problemId, int numOfSubmissions)
    {
        var userSubmissions = await _dbContext.UserSubmissions
            .OrderByDescending(us => us.Id)
            .Include(us => us.User)
            .Include(us => us.Problem)
            .Include(us => us.TestCases)
            .ThenInclude(tc => tc.TestCaseStatus)
            .Where(us => us.Course.CourseId == courseId && us.Problem.ProblemId == problemId)
            .Take(numOfSubmissions)
            .ToListAsync();
        return userSubmissions;
    }
}