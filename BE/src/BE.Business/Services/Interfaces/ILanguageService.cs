using BE.DTOs.DTOs.Language.Responses;

namespace BE.Business.Services.Interfaces;

public interface ILanguageService
{
    /// <summary>
    /// Retrieves a list of all available languages
    /// </summary>
    /// <returns>A list of language DTOs containing language ID and name</returns>
    Task<List<LanguageDto>> GetAllLanguagesAsync();
}