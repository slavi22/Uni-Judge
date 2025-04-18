using System.Security.Claims;
using BE.Business.Services.Interfaces;
using BE.DTOs.DTOs.Auth.Requests;
using BE.DTOs.DTOs.Auth.Responses;
using BE.DTOs.DTOs.JWT.Responses;
using BE.Presentation.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BE.Tests.Controllers.Auth;

// how to test IActionResult => https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/testing?view=aspnetcore-9.0#test-actionresultt
public class AuthControllerTests
{
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
        _jwtServiceMock = new Mock<IJwtService>();
        _controller = new AuthController(_authServiceMock.Object, _jwtServiceMock.Object);
    }

    [Fact]
    public async Task Login_ReturnsOk_WhenCredentialsAreValid()
    {
        // Arrange
        var loginDto = new LoginDto { Email = "user@example.com", Password = "password" };
        var tokenDto = new TokenDto { AccessToken = "access", RefreshToken = "refresh" };
        _authServiceMock.Setup(s => s.LoginUser(loginDto)).ReturnsAsync(tokenDto);
        // https://stackoverflow.com/a/58973035
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
    }

    [Fact]
    public async Task Login_ReturnsNotFound_WhenCredentialsAreInvalid()
    {
        // Arrange
        var loginDto = new LoginDto { Email = "user@example.com", Password = "wrongpassword" };
        _authServiceMock.Setup(s => s.LoginUser(loginDto)).ReturnsAsync((TokenDto)null);

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        var problemResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, problemResult.StatusCode);
    }

    [Fact]
    public async Task Register_ReturnsCreated_WhenRegistrationIsSuccessful()
    {
        // Arrange
        var registerDto = new RegisterDto { Email = "newuser@example.com", Password = "password" };
        _authServiceMock.Setup(s => s.RegisterUser(registerDto)).ReturnsAsync(true);

        // Act
        var result = await _controller.Register(registerDto);

        // Assert
        var createdResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
        Assert.Equal("User registered successfully.", createdResult.Value);
    }

    [Fact]
    public async Task Register_ReturnsBadRequest_WhenRegistrationFails()
    {
        // Arrange
        var registerDto = new RegisterDto { Email = "newuser@example.com", Password = "password" };
        _authServiceMock.Setup(s => s.RegisterUser(registerDto)).ReturnsAsync(false);

        // Act
        var result = await _controller.Register(registerDto);

        // Assert
        var problemResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, problemResult.StatusCode);
    }

    [Fact]
    public async Task RegisterTeacher_ReturnsCreated_WhenRegistrationIsSuccessful()
    {
        // Arrange
        var registerTeacherDto = new RegisterTeacherDto
            { Email = "newteacher@example.com", Password = "password", Secret = "secret" };
        _authServiceMock.Setup(s => s.RegisterTeacher(registerTeacherDto)).ReturnsAsync(true);

        // Act
        var result = await _controller.RegisterTeacher(registerTeacherDto);

        // Assert
        var createdResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
        Assert.Equal("Teacher registered successfully.", createdResult.Value);
    }

    [Fact]
    public async Task RegisterTeacher_ReturnsBadRequest_WhenRegistrationFails()
    {
        // Arrange
        var registerTeacherDto = new RegisterTeacherDto
            { Email = "newteacher@example.com", Password = "password", Secret = "secret" };
        _authServiceMock.Setup(s => s.RegisterTeacher(registerTeacherDto)).ReturnsAsync(false);

        // Act
        var result = await _controller.RegisterTeacher(registerTeacherDto);

        // Assert
        var problemResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, problemResult.StatusCode);
    }

    [Fact]
    public async Task UserInfo_ReturnsUserInfo_WhenUserIsAuthenticated()
    {
        // Arrange
        var email = "user@gmail.com";
        var userInfo = new UserInfoDto { Email = email };
        _authServiceMock.Setup(service => service.GetUserInfo(email)).ReturnsAsync(userInfo);
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, email) }, "mock"));
        _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

        // Act
        var result = await _controller.UserInfo();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(userInfo, okResult.Value);
    }

    [Fact]
    public async Task UserInfo_ReturnsNull_WhenUserIsNotAuthenticated()
    {
        // Arrange
        _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

        // Act
        var result = await _controller.UserInfo();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Null(okResult.Value);
    }
}