using BE.Business.Services.Interfaces;
using BE.Common.Exceptions;
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
        IProblemRepository problemRepository, ICourseRepository courseRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _userSubmissionRepository = userSubmissionRepository;
        _problemRepository = problemRepository;
        _courseRepository = courseRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UserSubmissionResultDto> AddUserSubmission(ClientSubmissionDto clientSubmissionDto,
        List<SubmissionBatchResultResponseDto> submissionBatchResultResponseDto)
    {
        // Here we fill the test cases list with the data from the response from the judge service
        // Then we create a new UserSubmissionModel entity and fill it with the data from the clientSubmissionDto and the test cases list
        // We then calculate if the submission is passing or not and add the entity to the database
        // After that we create a new UserSubmissionResultDto and fill it with the data from the entity and return it
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
        var userSubmissionDto = new UserSubmissionResultDto
        {
            SumbissionId = entity.Id,
        };
        foreach (var testCaseModel in testCasesList)
        {
            var testCaseResultId = testCaseModel.TestCaseStatus.ResultId;
            if (testCaseResultId >= 6)
            {
                userSubmissionDto.IsError = true;
            }

            userSubmissionDto.TestCases.Add(new TestCaseDto
            {
                IsCorrect = testCaseModel.IsCorrect,
                CompileOutput = testCaseModel.CompileOutput,
                ExpectedOutput = testCaseModel.ExpectedOutput,
                Stdout = testCaseModel.Stdout,
                Stderr = testCaseModel.Stderr,
                Status = new TestCaseStatusDto
                {
                    Id = testCaseResultId,
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
        var result = failedTestCases == 0 ? totalTestCases :
            totalTestCases == failedTestCases ? 0 : totalTestCases / failedTestCases;
        return result >= percentageInDecimal * totalTestCases;
    }

    //TODO: add test
    public async Task<List<ProblemUserSubmissionsDto>> GetUserSubmissionsForSpecificProblem(string courseId,
        string problemId)
    {
        var userEmail = _httpContextAccessor.HttpContext.User.Identity.Name;
        var currentUser = await _userRepository.GetCurrentUserAsync(userEmail);
        var userSubmissions =
            await _userSubmissionRepository.GetAllUserSubmissionsForSpecificProblemAsync(courseId, problemId,
                currentUser.Id);
        var allUserSubmissions = new List<ProblemUserSubmissionsDto>();


        foreach (var submission in userSubmissions)
        {
            var anyErrorTestCase = submission.TestCases.Any(tc => tc.TestCaseStatus.ResultId >= 6);
            var errorDescription = anyErrorTestCase
                ? submission.TestCases.FirstOrDefault(tc => tc.TestCaseStatus.ResultId >= 6)?.TestCaseStatus.Description
                : null;

            allUserSubmissions.Add(new ProblemUserSubmissionsDto
            {
                SubmissionId = submission.Id,
                IsPassing = submission.IsPassing,
                LanguageId = submission.LanguageId.ToString(),
                IsError = anyErrorTestCase,
                ErrorResult = errorDescription
            });
        }

        return allUserSubmissions;
    }

    //TODO: add test
    public async Task<List<TeacherLastUserSubmissionsDto>> GetLastUserSubmissionsForProblem(string courseId,
        string problemId, int numOfSubmissions)
    {
        // TODO: maybe make sure other teachers cant access info about courses/problems they havent created ?
        if (await _courseRepository.GetCourseByCourseIdAsync(courseId) == null)
        {
            throw new CourseNotFoundException("Course not found.");
        }

        if (await _problemRepository.GetProblemByProblemIdAsync(problemId) == null)
        {
            throw new ProblemNotFoundException("Problem not found.");
        }

        var lastUserSubmission =
            await _userSubmissionRepository.GetLastUserSubmissionsForProblemAsync(courseId, problemId,
                numOfSubmissions);
        var result = new List<TeacherLastUserSubmissionsDto>();
        foreach (var submission in lastUserSubmission)
        {
            var anyErrorTestCase = submission.TestCases.Any(tc => tc.TestCaseStatus.ResultId >= 6);
            var errorDescription = anyErrorTestCase
                ? submission.TestCases.FirstOrDefault(tc => tc.TestCaseStatus.ResultId >= 6)?.TestCaseStatus.Description
                : null;

            result.Add(new TeacherLastUserSubmissionsDto
            {
                SubmissionId = submission.Id,
                User = submission.User.Email,
                IsPassing = submission.IsPassing,
                IsError = anyErrorTestCase,
                ErrorResult = errorDescription,
                LanguageId = submission.LanguageId.ToString(),
                ProblemId = submission.Problem.ProblemId
            });
        }

        return result;
    }
}