using BE.Data;
using BE.Models.Problem;
using BE.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories.Implementations;

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
            .Include(problemModel => problemModel.ExpectedOutputList).FirstOrDefaultAsync(x => x.ProblemId == problemId);
    }

    public async Task AddProblemAsync(ProblemModel problem)
    {
        await _dbContext.Problems.AddAsync(problem);
        await _dbContext.SaveChangesAsync();
    }
}