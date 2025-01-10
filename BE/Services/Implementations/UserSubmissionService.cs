using BE.DTOs.Judge.Requests;
using BE.DTOs.Judge.Responses;
using BE.Models.Submissions;
using BE.Repositories.Interfaces;
using BE.Services.Interfaces;

namespace BE.Services.Implementations;

public class UserSubmissionService : IUserSubmissionService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserSubmissionRepository _userSubmissionRepository;
    private readonly IProblemRepository _problemRepository;

    public UserSubmissionService(IUserRepository userRepository, IUserSubmissionRepository userSubmissionRepository, IProblemRepository problemRepository)
    {
        _userRepository = userRepository;
        _userSubmissionRepository = userSubmissionRepository;
        _problemRepository = problemRepository;
    }

    public async Task AddUserSubmission(ClientSubmissionDto clientSubmissionDto, List<SubmissionBatchResultResponseDto> submissionBatchResultResponseDto)
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
        var entity = new UserSubmissionModel
        {
            LanguageId = int.Parse(clientSubmissionDto.LanguageId),
            ProblemId = clientSubmissionDto.ProblemId,
            SourceCode = clientSubmissionDto.SourceCode,
            UserId = appUser.Id,
            TestCases = testCasesList
        };

        await _userSubmissionRepository.AddAsync(entity);
    }

    private bool CalculateIsPassing(int problemId, List<TestCaseModel> testCasesList)
    {
        foreach (var testCase in testCasesList)
        {

        }
        return true;
    }
}