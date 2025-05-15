using BE.Business.Services.Interfaces;
using BE.Common.Exceptions;
using BE.DTOs.DTOs.Judge.Requests;
using BE.DTOs.DTOs.Judge.Responses;
using BE.DTOs.DTOs.UserSubmission.Responses;
using BE.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BE.Tests.Controllers.Submission;


// yt tutorial => https://www.youtube.com/watch?v=9ZvDBSQa_so
public class SubmissionControllerTests
{
    private readonly Mock<IJudgeService> _mockJudgeService;
    private readonly Mock<IUserSubmissionService> _mockUserSubmissionService;
    private readonly SubmissionController _controller;

    public SubmissionControllerTests()
    {
        _mockJudgeService = new Mock<IJudgeService>();
        _mockUserSubmissionService = new Mock<IUserSubmissionService>();
        _controller = new SubmissionController(_mockJudgeService.Object, _mockUserSubmissionService.Object);
    }

    [Fact]
    public async Task CreateSubmissionBatch_ReturnsOkResult_WithUserSubmissionDto()
    {
        // Arrange
        var clientSubmissionDto = new ClientSubmissionDto();
        var judgeResults = new List<SubmissionBatchResultResponseDto>();
        var userSubmissionDto = new UserSubmissionResultDto();

        _mockJudgeService.Setup(service => service.CreateBatchSubmissions(clientSubmissionDto))
            .ReturnsAsync(judgeResults);
        _mockUserSubmissionService
            .Setup(service => service.AddUserSubmission(It.IsAny<ClientSubmissionDto>(), judgeResults))
            .ReturnsAsync(userSubmissionDto);

        // Act
        var result = await _controller.CreateSubmissionBatch(clientSubmissionDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<UserSubmissionResultDto>(okResult.Value);
    }

    [Fact]
    public async Task CreateSubmissionBatch_ThrowsCourseNotFoundException_WhenCourseNotFound()
    {
        // Arrange
        _mockJudgeService.Setup(s => s.CreateBatchSubmissions(It.IsAny<ClientSubmissionDto>()))
            .ThrowsAsync(new CourseNotFoundException("Course not found."));

        // Act & Assert
        var dtoWithBadDetails = It.IsAny<ClientSubmissionDto>();
        await Assert.ThrowsAsync<CourseNotFoundException>(() =>
            _controller.CreateSubmissionBatch(dtoWithBadDetails));
    }

    [Fact]
    public async Task CreateSubmissionBatch_ThrowsProblemNotFound_WhenProblemNotFound()
    {
        // Arrange
        _mockJudgeService.Setup(s => s.CreateBatchSubmissions(It.IsAny<ClientSubmissionDto>()))
            .ThrowsAsync(new ProblemNotFoundException("Problem not found."));

        // Act & Assert
        var dtoWithBadDetails = It.IsAny<ClientSubmissionDto>();
        await Assert.ThrowsAsync<ProblemNotFoundException>(() =>
            _controller.CreateSubmissionBatch(dtoWithBadDetails));
    }
}