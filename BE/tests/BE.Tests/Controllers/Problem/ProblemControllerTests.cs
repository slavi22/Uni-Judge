using BE.Business.Services.Interfaces;
using BE.DTOs.DTOs.Problem.Requests;
using BE.DTOs.DTOs.Problem.Responses;
using BE.Presentation.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BE.Tests.Controllers.Problem;

// we cant test things such as the Authorize attribute because these type of test are generally not for unit tests but for integration tests
// https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/testing?view=aspnetcore-9.0#:~:text=Set%20up%20unit,NET%20Core.
public class ProblemControllerTests
{
    private readonly Mock<IProblemService> _mockProblemService;
    private readonly ProblemController _controller;

    public ProblemControllerTests()
    {
        _mockProblemService = new Mock<IProblemService>();
        _controller = new ProblemController(_mockProblemService.Object);
    }

    [Fact]
    public async Task CreateProblem_Returns200WithCreatedProblemDetails()
    {
        // Arrange
        var clientProblemDto = new ClientProblemDto { Name = "New Problem" };
        var createdProblem = new CreatedProblemDto { ProblemId = "problem123", Name = "New Problem" };
        _mockProblemService.Setup(service => service.CreateProblemAsync(clientProblemDto)).ReturnsAsync(createdProblem);

        // Act
        var result = await _controller.CreateProblem(clientProblemDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.Equal(createdProblem, okResult.Value);
    }

    [Fact]
    public async Task CreateProbmel_ThrowsException_WhenProblemServiceThrowsException()
    {
        // Arrange
        _mockProblemService.Setup(service => service.CreateProblemAsync(It.IsAny<ClientProblemDto>()))
            .ThrowsAsync(new Exception("An error occurred"));

        // Act & Assert
        var dtoWithBadDetails = It.IsAny<ClientProblemDto>();
        await Assert.ThrowsAsync<Exception>(() => _controller.CreateProblem(dtoWithBadDetails));
    }
}