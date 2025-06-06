using BE.Business.Services.Interfaces;
using BE.Common.Exceptions;
using BE.DataAccess.Repositories.Interfaces;
using BE.DTOs.DTOs.Problem.Requests;
using BE.DTOs.DTOs.Problem.Responses;
using BE.Models.Models.Problem;
using BE.Models.Models.Problem.Enums;
using Microsoft.AspNetCore.Http;

namespace BE.Business.Services.Implementations;

public class ProblemService : IProblemService
{
    private readonly IProblemRepository _problemRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProblemService(IProblemRepository problemRepository, ICourseRepository courseRepository,
        IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
    {
        _problemRepository = problemRepository;
        _courseRepository = courseRepository;
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    // Here we are creating a new problem.
    // First we have to map the MainMethodBodyDto to the MainMethodBody entity so we iterate over the list of MainMethodBodyDto and create a new MainMethodBody entity for each one
    // Then we create a new Problem entity and set its properties to the values from the ClientProblemDto
    // Then we iterate over the list of languages and create a new ProblemLanguage entity for each language and add it to the Problem entity
    public async Task<CreatedProblemDto> CreateProblemAsync(ClientProblemDto dto)
    {
        var problem = await _problemRepository.GetProblemByProblemIdAsync(dto.ProblemId);
        if (problem != null)
        {
            throw new DuplicateProblemIdException(
                $"A problem with the given ID - '{problem.ProblemId}' already exists.");
        }

        List<MainMethodBodyModel> mainMethodBodies = new List<MainMethodBodyModel>();
        foreach (var bodyDto in dto.MainMethodBodiesList)
        {
            mainMethodBodies.Add(new MainMethodBodyModel
            {
                LanguageId = bodyDto.LanguageId,
                MainMethodBodyContent = bodyDto.MainMethodBodyContent,
                SolutionTemplate = bodyDto.SolutionTemplate,
            });
        }

        var courseEntity = await _courseRepository.GetCourseByCourseIdAsync(dto.CourseId);
        var userEmail = _httpContextAccessor.HttpContext.User.Identity.Name;
        var currentUser = await _userRepository.GetCurrentUserAsync(userEmail);

        var problemEntity = new ProblemModel
        {
            ProblemId = dto.ProblemId,
            Name = dto.Name,
            Description = dto.Description,
            RequiredPercentageToPass = dto.RequiredPercentageToPass,
            CourseId = courseEntity.Id,
            MainMethodBodiesList = mainMethodBodies,
            User = currentUser
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
                IsSample = input.IsSample,
                StdIn = input.StdIn
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
            CourseId = courseEntity.CourseId
        };
        return createdProblemDto;
    }

    //TODO: add test
    public async Task<CreatedProblemDto> EditProblemAsync(string courseId, string problemId, ClientProblemDto dto)
    {
        var problem = await _problemRepository.GetProblemByProblemIdAndCourseId(courseId, problemId);
        problem.ProblemId = dto.ProblemId;
        problem.Name = dto.Name;
        problem.Description = dto.Description;
        problem.RequiredPercentageToPass = dto.RequiredPercentageToPass;
        problem.MainMethodBodiesList.Clear();
        foreach (var bodyDto in dto.MainMethodBodiesList)
        {
            problem.MainMethodBodiesList.Add(new MainMethodBodyModel
            {
                LanguageId = bodyDto.LanguageId,
                MainMethodBodyContent = bodyDto.MainMethodBodyContent,
                SolutionTemplate = bodyDto.SolutionTemplate
            });
        }

        problem.ExpectedOutputList.Clear();
        foreach (var testCase in dto.ExpectedOutputList)
        {
            var expectedOutputList = new ExpectedOutputListModel
            {
                ProblemId = problem.Id,
                IsSample = testCase.IsSample,
                ExpectedOutput = testCase.ExpectedOutput
            };
            problem.ExpectedOutputList.Add(expectedOutputList);
        }

        problem.StdInList.Clear();
        foreach (var input in dto.StdInList)
        {
            var stdInList = new StdInListModel
            {
                ProblemId = problem.Id,
                IsSample = input.IsSample,
                StdIn = input.StdIn
            };
            problem.StdInList.Add(stdInList);
        }

        // saw the clear method here => https://github.com/dotnet/efcore/issues/31033
        problem.ProblemLanguages.Clear();
        foreach (var languageId in dto.LanguagesList)
        {
            var problemLanguage = new ProblemLanguageModel
            {
                ProblemId = problem.Id,
                LanguageId = (int)languageId
            };
            problem.ProblemLanguages.Add(problemLanguage);
        }

        await _problemRepository.EditProblemAsync(problem);
        var updatedProblemDto = new CreatedProblemDto
        {
            ProblemId = problem.ProblemId,
            Name = problem.Name,
            Description = problem.Description,
            RequiredPercentageToPass = problem.RequiredPercentageToPass,
            CourseId = problem.CourseId
        };
        return updatedProblemDto;
    }

    //TODO: add test
    public async Task<List<TeacherProblemsDto>> GeyMyCreatedProblemsAsync()
    {
        var problemsList = new List<TeacherProblemsDto>();
        var userEmail = _httpContextAccessor.HttpContext.User.Identity.Name;
        var currentUser = await _userRepository.GetCurrentUserAsync(userEmail);
        var teacherProblems = await _problemRepository.GetTeacherProblems(currentUser.Id);
        foreach (var problem in teacherProblems)
        {
            problemsList.Add(new TeacherProblemsDto
            {
                CourseId = problem.Course.CourseId, ProblemId = problem.ProblemId, Name = problem.Name,
                Description = problem.Description
            });
        }

        return problemsList;
    }

    //TODO: add test
    public async Task<ProblemInfoDto> GetProblemInfoAsync(string courseId, string problemId)
    {
        var problem = await _problemRepository.GetProblemWithLanguagesAndMainMethodBodies(courseId, problemId);
        if (problem == null)
        {
            throw new ProblemNotFoundException($"The problem with ID '{problemId}' was not found.");
        }

        var solutionTemplates = new List<SolutionTemplateDto>();
        foreach (var mainMethodBody in problem.MainMethodBodiesList)
        {
            solutionTemplates.Add(new SolutionTemplateDto
            {
                LanguageId = ((int)mainMethodBody.LanguageId).ToString(),
                SolutionTemplateContent = mainMethodBody.SolutionTemplate
            });
        }

        var problemDto = new ProblemInfoDto
        {
            CourseId = problem.Course.CourseId,
            ProblemId = problem.ProblemId,
            Name = problem.Name,
            Description = problem.Description,
            RequiredPercentageToPass = problem.RequiredPercentageToPass,
            SolutionTemplates = solutionTemplates,
            ExpectedOutputList =
                problem.ExpectedOutputList.Where(e => e.IsSample).Select(e => e.ExpectedOutput).ToList(),
            StdInList = problem.StdInList.Where(s => s.IsSample).Select(s => s.StdIn).ToList(),
            AvailableLanguages = problem.ProblemLanguages.Select(p => (LanguagesEnum)p.LanguageId).ToList()
        };
        return problemDto;
    }

    //TODO: add test
    public async Task<EditProblemInfoDto> GetEditProblemInfoAsync(string courseId, string problemId)
    {
        var problem = await _problemRepository.GetProblemWithLanguagesAndMainMethodBodies(courseId, problemId);
        if (problem == null)
        {
            throw new ProblemNotFoundException($"The problem with ID '{problemId}' was not found.");
        }

        var editProblemInfoDto = new EditProblemInfoDto
        {
            CourseId = problem.Course.CourseId,
            ProblemId = problem.ProblemId,
            Name = problem.Name,
            Description = problem.Description,
            RequiredPercentageToPass = problem.RequiredPercentageToPass,
            MainMethodBodiesList = problem.MainMethodBodiesList.Select(m => new MainMethodBodyDto
            {
                LanguageId = m.LanguageId,
                MainMethodBodyContent = m.MainMethodBodyContent,
                SolutionTemplate = m.SolutionTemplate
            }).ToList(),
            ExpectedOutputList = problem.ExpectedOutputList.OrderBy(e => e.Id).Select(e => new ExpectedOutputListDto
            {
                IsSample = e.IsSample,
                ExpectedOutput = e.ExpectedOutput
            }).ToList(),
            StdInList = problem.StdInList.OrderBy(s => s.Id).Select(s => new StdInListDto
            {
                IsSample = s.IsSample,
                StdIn = s.StdIn
            }).ToList(),
            LanguagesList = problem.ProblemLanguages.Select(p => (LanguagesEnum)p.LanguageId).ToList()
        };

        return editProblemInfoDto;
    }
}