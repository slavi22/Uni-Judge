using BE.DataAccess.Data;
using BE.DataAccess.Repositories.Interfaces;
using BE.Models.Models.Problem;
using Microsoft.EntityFrameworkCore;

namespace BE.DataAccess.Repositories.Implementations;

public class MainMethodBodiesRepository : IMainMethodBodiesRepository
{
    private readonly AppDbContext _dbContext;

    public MainMethodBodiesRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MainMethodBodyModel> GetMainMethodBodyByIdAsync(string problemId)
    {
        return await _dbContext.MainMethodBodies.FirstOrDefaultAsync(x => x.ProblemId == problemId);
    }
}