using BE.Data;
using BE.DTOs.Problem;
using BE.Models.Problem;
using BE.Repositories;

namespace BE.Services;

public class ProblemService : IProblemService
{
    private readonly AppDbContext _dbContext;

    public ProblemService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    // Here we are creating a new problem.
    // First we have to map the MainMethodBodyDto to the MainMethodBody entity so we iterate over the list of MainMethodBodyDto and create a new MainMethodBody entity for each one
    // Then we create a new Problem entity and set its properties to the values from the CreateProblemDto
    // Then we iterate over the list of languages and create a new ProblemLanguage entity for each language and add it to the Problem entity
    public async Task<Problem> CreateProblem(CreateProblemDto dto)
    {
        List<MainMethodBody> mainMethodBodies = new List<MainMethodBody>();
        foreach (var bodyDto in dto.MainMethodBodiesList)
        {
            mainMethodBodies.Add(new MainMethodBody
            {
                Language = bodyDto.Language,
                MainMethodBodyContent = bodyDto.MainMethodBodyContent
            });
        }

        var problemEntity = new Problem
        {
            Name = dto.Name,
            Description = dto.Description,
            ExpectedOutputList = dto.ExpectedOutputList,
            StdInList = dto.StdInList,
            MainMethodBodiesList = mainMethodBodies
        };
        foreach (var languageId in dto.LanguagesList)
        {
            var problemLanguage = new ProblemLanguage
            {
                ProblemId = problemEntity.Id,
                LanguageId = (int)languageId
            };
            problemEntity.ProblemLanguages.Add(problemLanguage);
        }

        await _dbContext.Problems.AddAsync(problemEntity);
        await _dbContext.SaveChangesAsync();
        return problemEntity;
    }
}