using BE.Business.Services.Implementations;
using BE.Business.Services.Interfaces;
using BE.Common.Exceptions;
using BE.DataAccess.Repositories.Interfaces;
using BE.DTOs.DTOs.Auth.Requests;
using BE.Models.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;

namespace BE.Tests.Services.Auth;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _configurationMock = new Mock<IConfiguration>();
        _jwtServiceMock = new Mock<IJwtService>();
        _authService = new AuthService(_userRepositoryMock.Object, _configurationMock.Object, _jwtServiceMock.Object);
    }

    [Fact]
    public async Task LoginUser_WithValidCredentials_ReturnsTokenDto()
    {
        // Arrange
        var loginDto = new LoginDto { Email = "test@example.com", Password = "Password123" };
        _userRepositoryMock.Setup(repo => repo.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false)).ReturnsAsync(true);
        var user = new AppUser { UserName = loginDto.Email, Email = loginDto.Email };
        _userRepositoryMock.Setup(repo => repo.FindByNameAsync(loginDto.Email)).ReturnsAsync(user);
        _jwtServiceMock.Setup(service => service.GenerateJwtToken(user)).ReturnsAsync("accessToken");
        _jwtServiceMock.Setup(service => service.GenerateRefreshToken()).Returns("refreshToken");

        // Act
        var result = await _authService.LoginUser(loginDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("accessToken", result.AccessToken);
        Assert.Equal("refreshToken", result.RefreshToken);
    }

    [Fact]
    public async Task LoginUser_WithInvalidCredentials_ReturnsNull()
    {
        // Arrange
        var loginDto = new LoginDto { Email = "test@example.com", Password = "WrongPassword" };
        _userRepositoryMock.Setup(repo => repo.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false)).ReturnsAsync(false);

        // Act
        var result = await _authService.LoginUser(loginDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task RegisterUser_WithValidData_ReturnsTrue()
    {
        // Arrange
        var registerDto = new RegisterDto { Email = "test@example.com", Password = "Password123" };
        var resultDto = IdentityResult.Success;
        _userRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<AppUser>(), registerDto.Password)).ReturnsAsync(resultDto);
        _userRepositoryMock.Setup(repo => repo.GetUserCountAsync()).ReturnsAsync(1);

        // Act
        var result = await _authService.RegisterUser(registerDto);

        // Assert
        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task RegisterUser_WithValidData_AddsStudentRole()
    {
        // Arrange
        var registerDto = new RegisterDto { Email = "test@example.com", Password = "Password123" };
        _userRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<AppUser>(), registerDto.Password)).ReturnsAsync(IdentityResult.Success);
        _userRepositoryMock.Setup(repo => repo.GetUserCountAsync()).ReturnsAsync(2);

        // Act
        var result = await _authService.RegisterUser(registerDto);

        // Assert
        _userRepositoryMock.Verify(repo => repo.AddToRoleAsync(It.IsAny<AppUser>(), "Student"), Times.Once);
    }

    [Fact]
    public async Task RegisterTeacher_WithValidSecret_ReturnsTrue()
    {
        // Arrange
        var registerTeacherDto = new RegisterTeacherDto { Email = "teacher@example.com", Password = "Password123", Secret = "ValidSecret" };
        var resultDto = IdentityResult.Success;
        _configurationMock.Setup(config => config.GetSection("TeacherSecret").Value).Returns("ValidSecret");
        _userRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<AppUser>(), registerTeacherDto.Password)).ReturnsAsync(resultDto);

        // Act
        var result = await _authService.RegisterTeacher(registerTeacherDto);

        // Assert
        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task RegisterTeacher_WithInvalidSecret_ThrowsIncorrectTeacherSecretException()
    {
        // Arrange
        var registerTeacherDto = new RegisterTeacherDto { Email = "teacher@example.com", Password = "Password123", Secret = "InvalidSecret" };
        _configurationMock.Setup(config => config.GetSection("TeacherSecret").Value).Returns("ValidSecret");

        // Act & Assert
        await Assert.ThrowsAsync<IncorrectTeacherSecretException>(() => _authService.RegisterTeacher(registerTeacherDto));
    }
}