using BE.DataAccess.Data;
using BE.DataAccess.Repositories.Interfaces;
using BE.Models.Models.Problem;
using Microsoft.EntityFrameworkCore;

namespace BE.DataAccess.Repositories.Implementations;

public class LanguageRepository : ILanguageRepository
{
    private readonly AppDbContext _dbContext;

    public LanguageRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<LanguageModel> GetLanguageByIdAsync(int languageId)
    {
        return await _dbContext.Languages.FirstOrDefaultAsync(x => x.Id == languageId);
    }
}