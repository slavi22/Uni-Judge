using BE.Business.Services.Interfaces;
using BE.DTOs.DTOs.Course.Requests;
using BE.DTOs.DTOs.Course.Responses;
using BE.Presentation.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BE.Tests.Controllers.Course;

public class CourseControllerTests
{
    private readonly Mock<ICourseService> _courseServiceMock;
    private readonly CourseController _controller;

    public CourseControllerTests()
    {
        _courseServiceMock = new Mock<ICourseService>();
        _controller = new CourseController(_courseServiceMock.Object);
    }

    [Fact]
    public async Task GetCourseProblems_ReturnsOk_WithCourseProblems()
    {
        // Arrange
        var courseId = "course123";
        var courseProblems = new List<ViewCourseProblemDto> { new ViewCourseProblemDto { ProblemId = "problem1" } };
        _courseServiceMock.Setup(s => s.GetProblemsForCourse(courseId)).ReturnsAsync(courseProblems);

        // Act
        var result = await _controller.GetCourseProblems(courseId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.Equal(courseProblems, okResult.Value);
    }

    [Fact]
    public async Task GetCourseProblems_ReturnsNotFound_WhenCourseDoesNotExist()
    {
        // Arrange
        var courseId = "nonexistentCourse";
        _courseServiceMock.Setup(s => s.GetProblemsForCourse(courseId)).ReturnsAsync((List<ViewCourseProblemDto>)null);

        // Act
        var result = await _controller.GetCourseProblems(courseId);

        // Assert
        var problemResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, problemResult.StatusCode);
    }

    [Fact]
    public async Task SignUpForCourse_ReturnsOk_WhenSignUpIsSuccessful()
    {
        // Arrange
        var signUpDto = new SignUpForCourseDto { CourseId = "course123" };
        _courseServiceMock.Setup(s => s.SignUpForCourse(signUpDto)).ReturnsAsync(true);

        // Act
        var result = await _controller.SignUpForCourse(signUpDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.Equal("Successfully signed up for course", okResult.Value);
    }

    [Fact]
    public async Task SignUpForCourse_ReturnsBadRequest_WhenSignUpFails()
    {
        // Arrange
        var signUpDto = new SignUpForCourseDto { CourseId = "course123" };
        _courseServiceMock.Setup(s => s.SignUpForCourse(signUpDto)).ReturnsAsync(false);

        // Act
        var result = await _controller.SignUpForCourse(signUpDto);

        // Assert
        var problemResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, problemResult.StatusCode);
    }


    [Fact]
    public async Task CreateCourse_ReturnsOk_WhenCourseIsCreated()
    {
        // Arrange
        var createCourseDto = new CreateCourseDto { Name = "New Course" };
        _courseServiceMock.Setup(s => s.CreateNewCourse(createCourseDto)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.CreateCourse(createCourseDto);

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
    }
}