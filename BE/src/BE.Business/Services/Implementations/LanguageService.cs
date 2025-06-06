using BE.Business.Services.Interfaces;
using BE.DataAccess.Repositories.Interfaces;
using BE.DTOs.DTOs.Language.Responses;

namespace BE.Business.Services.Implementations;

public class LanguageService : ILanguageService
{
    private readonly ILanguageRepository _languageRepository;

    public LanguageService(ILanguageRepository languageRepository)
    {
        _languageRepository = languageRepository;
    }

    public async Task<List<LanguageDto>> GetAllLanguagesAsync()
    {
        var languagesList = new List<LanguageDto>();
        var languagesInTheSystem = await _languageRepository.GetAllLanguagesAsync();
        foreach (var language in languagesInTheSystem)
        {
            languagesList.Add(new LanguageDto
            {
                LanguageId = language.Id,
                Name = language.Name
            });
        }

        return languagesList;
    }
}