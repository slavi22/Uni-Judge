using BE.DataAccess.Repositories.Interfaces;
using BE.DTOs.DTOs.Judge.Requests;
using BE.DTOs.DTOs.Judge.Responses;
using BE.Models.Models.Auth;
using BE.Models.Models.Courses;
using BE.Models.Models.Problem;
using BE.Models.Models.Submissions;
using Moq;

namespace BE.Tests.Services.UserSubmissionService;

public class UserSubmissionServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUserSubmissionRepository> _userSubmissionRepositoryMock;
    private readonly Mock<IProblemRepository> _problemRepositoryMock;
    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly Business.Services.Implementations.UserSubmissionService _userSubmissionService;

    public UserSubmissionServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userSubmissionRepositoryMock = new Mock<IUserSubmissionRepository>();
        _problemRepositoryMock = new Mock<IProblemRepository>();
        _courseRepositoryMock = new Mock<ICourseRepository>();

        _userSubmissionService = new Business.Services.Implementations.UserSubmissionService(
            _userRepositoryMock.Object,
            _userSubmissionRepositoryMock.Object,
            _problemRepositoryMock.Object,
            _courseRepositoryMock.Object
        );
    }

    [Fact]
    public async Task AddUserSubmission_ShouldReturnUserSubmissionDto_WhenSubmissionIsValid()
    {
        var clientSubmissionDto = new ClientSubmissionDto
        {
            LanguageId = "1",
            ProblemId = "problem1",
            CourseId = "course1",
            SourceCode = "source code"
        };

        var submissionBatchResultResponseDto = new List<SubmissionBatchResultResponseDto>
        {
            new SubmissionBatchResultResponseDto
            {
                Token = "token1",
                ExpectedOutput = "output1",
                HiddenExpectedOutput = "hiddenOutput1",
                Stderr = "stderr1",
                Stdout = "stdout1",
                CompileOutput = "compileOutput1",
                IsCorrect = true,
                Status = new StatusDto { Id = 1, Description = "Success" }
            }
        };

        _userRepositoryMock.Setup(repo => repo.GetCurrentUserAsync()).ReturnsAsync(new AppUser { Id = "user1" });
        _problemRepositoryMock.Setup(repo => repo.GetProblemByProblemIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new ProblemModel { Id = "problem1", RequiredPercentageToPass = 80 });
        _courseRepositoryMock.Setup(repo => repo.GetCourseByCourseIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new CoursesModel { Id = "course1" });
        // callback => https://stackoverflow.com/a/34802674
        _userSubmissionRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<UserSubmissionModel>()))
            .Callback<UserSubmissionModel>(submission => submission.Id = Guid.NewGuid().ToString())
            .Returns(Task.CompletedTask);

        var result =
            await _userSubmissionService.AddUserSubmission(clientSubmissionDto, submissionBatchResultResponseDto);

        Assert.NotNull(result);
        Assert.NotEmpty(result.SumbissionId);
        Assert.NotEmpty(result.TestCases);
        Assert.Single(result.TestCases);
    }

    [Fact]
    public async Task AddUserSubmission_ShouldReturnUserSubmissionDto_WhenSubmissionIsInvalid()
    {
        var clientSubmissionDto = new ClientSubmissionDto
        {
            LanguageId = "1",
            ProblemId = "problem1",
            CourseId = "course1",
            SourceCode = "source code"
        };

        var submissionBatchResultResponseDto = new List<SubmissionBatchResultResponseDto>
        {
            new SubmissionBatchResultResponseDto
            {
                Token = "token1",
                ExpectedOutput = "output1",
                HiddenExpectedOutput = "hiddenOutput1",
                Stderr = "stderr1",
                Stdout = "stdout1",
                CompileOutput = "compileOutput1",
                IsCorrect = false,
                Status = new StatusDto { Id = 1, Description = "Failed" }
            }
        };

        _userRepositoryMock.Setup(repo => repo.GetCurrentUserAsync()).ReturnsAsync(new AppUser { Id = "user1" });
        _problemRepositoryMock.Setup(repo => repo.GetProblemByProblemIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new ProblemModel { Id = "problem1", RequiredPercentageToPass = 80 });
        _courseRepositoryMock.Setup(repo => repo.GetCourseByCourseIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new CoursesModel { Id = "course1" });
        _userSubmissionRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<UserSubmissionModel>()))
            .Callback<UserSubmissionModel>(submission => submission.Id = Guid.NewGuid().ToString())
            .Returns(Task.CompletedTask);

        var result =
            await _userSubmissionService.AddUserSubmission(clientSubmissionDto, submissionBatchResultResponseDto);

        Assert.NotNull(result);
        Assert.NotEmpty(result.SumbissionId);
        Assert.NotEmpty(result.TestCases);
        Assert.Single(result.TestCases);
    }
}