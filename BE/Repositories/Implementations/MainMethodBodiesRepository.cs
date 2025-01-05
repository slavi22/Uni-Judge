using BE.Data;
using BE.Models.Problem;
using BE.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories.Implementations;

public class MainMethodBodiesRepository : IMainMethodBodiesRepository
{
    private readonly AppDbContext _dbContext;

    public MainMethodBodiesRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MainMethodBodyModel> GetMainMethodBodyByIdAsync(int id)
    {
        return await _dbContext.MainMethodBodies.FirstOrDefaultAsync(x => x.ProblemId == id);
    }
}