using BE.DTOs.Problem;
using BE.Models.Problem;
using BE.Repositories.Implementations;
using BE.Repositories.Interfaces;
using BE.Services.Interfaces;

namespace BE.Services.Implementations;

public class ProblemService : IProblemService
{
    private readonly IProblemRepository _problemRepository;
    public ProblemService(IProblemRepository problemRepository)
    {
        _problemRepository = problemRepository;
    }


    // Here we are creating a new problem.
    // First we have to map the MainMethodBodyDto to the MainMethodBody entity so we iterate over the list of MainMethodBodyDto and create a new MainMethodBody entity for each one
    // Then we create a new Problem entity and set its properties to the values from the ClientProblemDto
    // Then we iterate over the list of languages and create a new ProblemLanguage entity for each language and add it to the Problem entity
    public async Task<CreatedProblemDto> CreateProblem(ClientProblemDto dto)
    {
        List<MainMethodBodyModel> mainMethodBodies = new List<MainMethodBodyModel>();
        foreach (var bodyDto in dto.MainMethodBodiesList)
        {
            mainMethodBodies.Add(new MainMethodBodyModel
            {
                Language = bodyDto.Language,
                MainMethodBodyContent = bodyDto.MainMethodBodyContent,
                SolutionTemplate = bodyDto.SolutionTemplate,
            });
        }

        var problemEntity = new ProblemModel
        {
            ProblemId = dto.ProblemId,
            Name = dto.Name,
            Description = dto.Description,
            RequiredPercentageToPass = dto.RequiredPercentageToPass,
            CourseId = dto.CourseId,
            MainMethodBodiesList = mainMethodBodies
        };

        foreach (var testCase in dto.ExpectedOutputList)
        {
            var expectedOutputList = new ExpectedOutputListModel
            {
                ProblemId = problemEntity.Id,
                IsSample = testCase.IsSample,
                ExpectedOutput = testCase.ExpectedOutput
            };
            problemEntity.ExpectedOutputList.Add(expectedOutputList);
        }

        foreach (var input in dto.StdInList)
        {
            var stdInList = new StdInListModel
            {
                ProblemId = problemEntity.Id,
                StdIn = input
            };
            problemEntity.StdInList.Add(stdInList);
        }

        
        foreach (var languageId in dto.LanguagesList)
        {
            var problemLanguage = new ProblemLanguageModel
            {
                ProblemId = problemEntity.Id,
                LanguageId = (int)languageId
            };
            problemEntity.ProblemLanguages.Add(problemLanguage);
        }

        await _problemRepository.AddProblemAsync(problemEntity);
        var createdProblemDto = new CreatedProblemDto
        {
            ProblemId = problemEntity.ProblemId,
            Name = problemEntity.Name,
            Description = problemEntity.Description,
            RequiredPercentageToPass = problemEntity.RequiredPercentageToPass,
            CourseId = problemEntity.CourseId
        };
        return createdProblemDto;
    }
}