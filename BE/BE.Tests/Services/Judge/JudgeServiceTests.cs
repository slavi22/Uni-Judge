using System.Net;
using System.Text;
using BE.Business.Services.Implementations;
using BE.Common.Exceptions;
using BE.DataAccess.Repositories.Interfaces;
using BE.DTOs.DTOs.Judge.Requests;
using BE.DTOs.DTOs.Judge.Responses;
using BE.Models.Models.Courses;
using BE.Models.Models.Problem;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace BE.Tests.Services.Judge;

public class JudgeServiceTests
{
    private readonly Mock<ICourseRepository> _courseRepository;
    private readonly Mock<IProblemRepository> _problemRepository;
    private readonly Mock<IMainMethodBodiesRepository> _mainMethodBodiesRepository;
    private readonly Mock<HttpMessageHandler> _httpMessageHandler;
    private readonly Mock<IHttpClientFactory> _httpClientFactory;

    public JudgeServiceTests()
    {
        _courseRepository = new Mock<ICourseRepository>();
        _problemRepository = new Mock<IProblemRepository>();
        _mainMethodBodiesRepository = new Mock<IMainMethodBodiesRepository>();
        _httpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClientFactory = new Mock<IHttpClientFactory>();
    }

    // how to mock httpclient => https://www.youtube.com/watch?v=7OFZZAHGv9o
    [Fact]
    public async Task CreateBatchSubmissions_ReturnsCorrectResults()
    {
        // Arrange
        var clientSubmissionDto = new ClientSubmissionDto
        {
            CourseId = "course1",
            ProblemId = "problem1",
            LanguageId = "51",
            SourceCode = "public class Solution { static void Main() { } }"
        };

        var problemEntity = new ProblemModel
        {
            Id = "problem1",
            ExpectedOutputList = new List<ExpectedOutputListModel>
            {
                new ExpectedOutputListModel { IsSample = true, ExpectedOutput = "output1" },
                new ExpectedOutputListModel { IsSample = false, ExpectedOutput = "hiddenOutput1" }
            },
            StdInList = new List<StdInListModel> { new StdInListModel { StdIn = "input1" } }
        };

        var mainMethodBodyEntity = new MainMethodBodyModel
        {
            Id = 1,
            MainMethodBodyContent = "public class Solution { static void Main() { } }"
        };

        var judgeResponse = new List<SubmissionResponseTokenDto>
        {
            new SubmissionResponseTokenDto { Token = "token1" }
        };

        var submissionStatuses = new SubmissionResultDto
        {
            Submissions = new List<SubmissionStatusDto>
            {
                new SubmissionStatusDto
                {
                    Status = new StatusDto { Id = 3 },
                    Stdout = Convert.ToBase64String(Encoding.UTF8.GetBytes("output1\n")),
                    Stderr = ""
                }
            }
        };

        _courseRepository.Setup(x => x.GetCourseByCourseIdAsync(It.IsAny<string>())).ReturnsAsync(new CoursesModel());
        _problemRepository.Setup(x => x.GetProblemByProblemIdAsync(It.IsAny<string>())).ReturnsAsync(problemEntity);
        _mainMethodBodiesRepository.Setup(x => x.GetMainMethodBodyByIdAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(mainMethodBodyEntity);

        _httpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(judgeResponse))
            });

        _httpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(submissionStatuses))
            });

        var httpClient = new HttpClient(_httpMessageHandler.Object)
        {
            BaseAddress = new Uri("http://test-address.com")
        };
        _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var service = new JudgeService(_mainMethodBodiesRepository.Object, _problemRepository.Object,
            _courseRepository.Object, _httpClientFactory.Object);

        // Act
        var result = await service.CreateBatchSubmissions(clientSubmissionDto);

        // Assert
        Assert.Single(result);
        Assert.True(result[0].IsCorrect);
        Assert.Equal("token1", result[0].Token);
        Assert.Empty(result[0].Stdout);
        Assert.Equal("output1", result[0].ExpectedOutput);
        Assert.Null(result[0].HiddenExpectedOutput);
        Assert.Empty(result[0].Stderr);
    }

    [Fact]
    public async Task CreateBatchSubmissions_ThrowsCourseNotFoundException()
    {
        // Arrange
        var clientSubmissionDto = new ClientSubmissionDto
        {
            CourseId = "invalidCourseId",
            ProblemId = "problem1",
            LanguageId = "51",
            SourceCode = "public class Solution { static void Main() { } }"
        };

        _courseRepository.Setup(x => x.GetCourseByCourseIdAsync(It.IsAny<string>())).ReturnsAsync((CoursesModel)null);

        var service = new JudgeService(_mainMethodBodiesRepository.Object, _problemRepository.Object,
            _courseRepository.Object, null);

        // Act & Assert
        await Assert.ThrowsAsync<CourseNotFoundException>(() => service.CreateBatchSubmissions(clientSubmissionDto));
    }

    [Fact]
    public async Task CreateBatchSubmissions_ThrowsProblemNotFoundException()
    {
        // Arrange
        var clientSubmissionDto = new ClientSubmissionDto
        {
            CourseId = "course1",
            ProblemId = "invalidProblemId",
            LanguageId = "51",
            SourceCode = "public class Solution { static void Main() { } }"
        };

        _courseRepository.Setup(x => x.GetCourseByCourseIdAsync(It.IsAny<string>())).ReturnsAsync(new CoursesModel());
        _problemRepository.Setup(x => x.GetProblemByProblemIdAsync(It.IsAny<string>()))
            .ReturnsAsync((ProblemModel)null);

        var service = new JudgeService(_mainMethodBodiesRepository.Object, _problemRepository.Object,
            _courseRepository.Object, null);

        // Act & Assert
        await Assert.ThrowsAsync<ProblemNotFoundException>(() => service.CreateBatchSubmissions(clientSubmissionDto));
    }
}