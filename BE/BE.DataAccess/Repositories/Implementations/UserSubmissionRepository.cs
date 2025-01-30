using BE.DataAccess.Data;
using BE.DataAccess.Repositories.Interfaces;
using BE.Models.Models.Submissions;

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
}