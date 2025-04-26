using BE.DataAccess.Data;
using BE.DataAccess.Repositories.Interfaces;
using BE.Models.Models.Problem;
using Microsoft.EntityFrameworkCore;

namespace BE.DataAccess.Repositories.Implementations;

public class ProblemRepository : IProblemRepository
{
    private readonly AppDbContext _dbContext;

    public ProblemRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ProblemModel> GetProblemByProblemIdAsync(string problemId)
    {
        return await _dbContext.Problems.Include(problemModel => problemModel.StdInList)
            .Include(problemModel => problemModel.ExpectedOutputList)
            .FirstOrDefaultAsync(x => x.ProblemId == problemId);
    }

    public async Task AddProblemAsync(ProblemModel problem)
    {
        await _dbContext.Problems.AddAsync(problem);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<ProblemModel>> GetTeacherProblems(string teacherId)
    {
        var teacherProblems =
            await _dbContext.Problems.Include(p => p.User).Include(p => p.Course).Where(p => p.UserId == teacherId)
                .ToListAsync();
        return teacherProblems;
    }
}