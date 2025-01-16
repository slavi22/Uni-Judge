using BE.DTOs.Judge.Requests;
using BE.DTOs.Judge.Responses;
using BE.DTOs.UserSubmission;
using BE.Models.Submissions;
using BE.Repositories.Interfaces;
using BE.Services.Interfaces;

namespace BE.Services.Implementations;

public class UserSubmissionService : IUserSubmissionService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserSubmissionRepository _userSubmissionRepository;
    private readonly IProblemRepository _problemRepository;

    public UserSubmissionService(IUserRepository userRepository, IUserSubmissionRepository userSubmissionRepository,
        IProblemRepository problemRepository)
    {
        _userRepository = userRepository;
        _userSubmissionRepository = userSubmissionRepository;
        _problemRepository = problemRepository;
    }

    public async Task<UserSubmissionDto> AddUserSubmission(ClientSubmissionDto clientSubmissionDto,
        List<SubmissionBatchResultResponseDto> submissionBatchResultResponseDto)
    {
        var testCasesList = new List<TestCaseModel>();
        foreach (var submissionBatchResultResponse in submissionBatchResultResponseDto)
        {
            var testCase = new TestCaseModel
            {
                Id = Guid.Parse(submissionBatchResultResponse.Token),
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

        var appUser = await _userRepository.GetCurrentUserAsync();
        var problem = await _problemRepository.GetProblemByProblemIdAsync(clientSubmissionDto.ProblemId);
        var entity = new UserSubmissionModel
        {
            LanguageId = int.Parse(clientSubmissionDto.LanguageId),
            CourseId = clientSubmissionDto.CourseId,
            ProblemId = problem.Id,
            SourceCode = clientSubmissionDto.SourceCode,
            IsPassing = await CalculateIsPassing(clientSubmissionDto.ProblemId, testCasesList),
            UserId = appUser.Id,
            TestCases = testCasesList
        };

        await _userSubmissionRepository.AddAsync(entity);
        //TODO: Investigate if the token generated normally
        var userSubmissionDto = new UserSubmissionDto
        {
            Token = entity.Id.ToString()
        };
        foreach (var testCaseModel in testCasesList)
        {
            userSubmissionDto.TestCases.Add(new TestCaseDto
            {
                IsCorrect = testCaseModel.IsCorrect,
                CompileOutput = testCaseModel.CompileOutput,
                ExpectedOutput = testCaseModel.ExpectedOutput,
                Stdout = testCaseModel.Stdout,
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
        var problem = await _problemRepository.GetProblemByProblemIdAsync(problemId);
        var percentageToPass = problem.RequiredPercentageToPass;
        var percentageInDecimal = (double)percentageToPass / 100;
        var failedTestCases = testCasesList.Count(x => x.IsCorrect == false);
        var totalTestCases = testCasesList.Count;
        var result = totalTestCases / failedTestCases;
        return result >= percentageInDecimal * totalTestCases;
    }
}