using BE.Models.Models.Problem;

namespace BE.DataAccess.Repositories.Interfaces;

public interface ILanguageRepository
{
    /// <summary>
    /// Retrieves a language by its ID.
    /// </summary>
    /// <param name="languageId">The ID of the language to retrieve</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the language model</returns>
    public Task<LanguageModel> GetLanguageByIdAsync(int languageId);
}