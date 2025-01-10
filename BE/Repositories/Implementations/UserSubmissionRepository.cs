using BE.Data;
using BE.Models.Submissions;
using BE.Repositories.Interfaces;

namespace BE.Repositories.Implementations;

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
}