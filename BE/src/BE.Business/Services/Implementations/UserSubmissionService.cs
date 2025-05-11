using BE.Business.Services.Interfaces;
using BE.DataAccess.Repositories.Interfaces;
using BE.DTOs.DTOs.Judge.Requests;
using BE.DTOs.DTOs.Judge.Responses;
using BE.DTOs.DTOs.UserSubmission.Responses;
using BE.Models.Models.Submissions;
using Microsoft.AspNetCore.Http;

namespace BE.Business.Services.Implementations;

public class UserSubmissionService : IUserSubmissionService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserSubmissionRepository _userSubmissionRepository;
    private readonly IProblemRepository _problemRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserSubmissionService(IUserRepository userRepository, IUserSubmissionRepository userSubmissionRepository,
        IProblemRepository problemRepository, ICourseRepository courseRepository, IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _userSubmissionRepository = userSubmissionRepository;
        _problemRepository = problemRepository;
        _courseRepository = courseRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UserSubmissionDto> AddUserSubmission(ClientSubmissionDto clientSubmissionDto,
        List<SubmissionBatchResultResponseDto> submissionBatchResultResponseDto)
    {
        // Here we fill the test cases list with the data from the response from the judge service
        // Then we create a new UserSubmissionModel entity and fill it with the data from the clientSubmissionDto and the test cases list
        // We then calculate if the submission is passing or not and add the entity to the database
        // After that we create a new UserSubmissionDto and fill it with the data from the entity and return it
        var testCasesList = new List<TestCaseModel>();
        foreach (var submissionBatchResultResponse in submissionBatchResultResponseDto)
        {
            var testCase = new TestCaseModel
            {
                Id = submissionBatchResultResponse.Token,
                ExpectedOutput = submissionBatchResultResponse.ExpectedOutput,
                HiddenExpectedOutput = submissionBatchResultResponse.HiddenExpectedOutput,
                Stderr = submissionBatchResultResponse.Stderr,
                Stdout = submissionBatchResultResponse.Stdout,
                CompileOutput = submissionBatchResultResponse.CompileOutput,
                IsCorrect = submissionBatchResultResponse.IsCorrect,
                TestCaseStatus = new TestCaseStatusModel
                {
                    ResultId = submissionBatchResultResponse.Status.Id,
                    Description = submissionBatchResultResponse.Status.Description
                }
            };
            testCasesList.Add(testCase);
        }

        var userEmail = _httpContextAccessor.HttpContext.User.Identity.Name;
        var appUser = await _userRepository.GetCurrentUserAsync(userEmail);
        var problem = await _problemRepository.GetProblemByProblemIdAsync(clientSubmissionDto.ProblemId);
        var course = await _courseRepository.GetCourseByCourseIdAsync(clientSubmissionDto.CourseId);
        var entity = new UserSubmissionModel
        {
            LanguageId = int.Parse(clientSubmissionDto.LanguageId),
            CourseId = course.Id,
            ProblemId = problem.Id,
            SourceCode = clientSubmissionDto.SourceCode,
            IsPassing = await CalculateIsPassing(clientSubmissionDto.ProblemId, testCasesList),
            UserId = appUser.Id,
            TestCases = testCasesList
        };

        await _userSubmissionRepository.AddAsync(entity);
        var userSubmissionDto = new UserSubmissionDto
        {
            SumbissionId = entity.Id
        };
        foreach (var testCaseModel in testCasesList)
        {
            userSubmissionDto.TestCases.Add(new TestCaseDto
            {
                IsCorrect = testCaseModel.IsCorrect,
                CompileOutput = testCaseModel.CompileOutput,
                ExpectedOutput = testCaseModel.ExpectedOutput,
                Stdout = testCaseModel.Stdout,
                Stderr = testCaseModel.Stderr,
                Status = new TestCaseStatusDto
                {
                    Id = testCaseModel.TestCaseStatus.ResultId,
                    Description = testCaseModel.TestCaseStatus.Description
                }
            });
        }

        return userSubmissionDto;
    }

    private async Task<bool> CalculateIsPassing(string problemId, List<TestCaseModel> testCasesList)
    {
        // Here we calculate if the submission is passing or not
        // We calculate it by getting the required percentage to pass from the problem then checking if the failed test cases are less than the required percentage to pass
        var problem = await _problemRepository.GetProblemByProblemIdAsync(problemId);
        var percentageToPass = problem.RequiredPercentageToPass;
        var percentageInDecimal = (double)percentageToPass / 100;
        var failedTestCases = testCasesList.Count(x => x.IsCorrect == false);
        var totalTestCases = testCasesList.Count;
        var result = failedTestCases == 0 ? totalTestCases : totalTestCases / failedTestCases;
        return result >= percentageInDecimal * totalTestCases;
    }
}