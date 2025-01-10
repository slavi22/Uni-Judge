using BE.Data;
using BE.Models.Problem;
using BE.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories.Implementations;

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