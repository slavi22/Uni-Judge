using BE.Models.Problem;

namespace BE.Repositories.Interfaces;

public interface ILanguageRepository
{
    public Task<LanguageModel> GetLanguageByIdAsync(int languageId);
}