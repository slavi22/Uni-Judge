using BE.Business.Services.Implementations;
using BE.DataAccess.Repositories.Interfaces;
using BE.DTOs.DTOs.JWT.Responses;
using BE.Models.Models.Auth;
using Microsoft.Extensions.Configuration;
using Moq;

namespace BE.Tests.Services.Jwt;

public class JwtServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly JwtService _jwtService;

    public JwtServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _configurationMock = new Mock<IConfiguration>();
        _jwtService = new JwtService(_userRepositoryMock.Object, _configurationMock.Object);
    }

    [Fact]
    public async Task GenerateAccessTokenFromRefreshToken_ValidRefreshToken_ReturnsNewAccessToken()
    {
        // Arrange
        var user = new AppUser
        {
            UserName = "testuser", RefreshToken = "validRefreshToken",
            RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(5)
        };
        var tokenDto = new TokenDto
        {
            AccessToken =
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InRlc3R1c2VyQHRlc3QuY29tIiwicm9sZSI6WyJTdHVkZW50Il19.DV1wVmy27XWi4YLQB2t3_PbL6TzBrqlZfaU4d6VgcTI",
            RefreshToken = "validRefreshToken"
        };

        _userRepositoryMock.Setup(repo => repo.FindByNameAsync("testuser@test.com")).ReturnsAsync(user);
        _userRepositoryMock.Setup(repo => repo.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Student" });
        _configurationMock.Setup(config => config.GetSection("JWT:Secret").Value)
            .Returns("supersecretkey123supersecretkey123");
        _configurationMock.Setup(config => config.GetSection("JWT:Issuer").Value).Returns("issuer");
        _configurationMock.Setup(config => config.GetSection("JWT:Audience").Value).Returns("audience");

        // Act
        var result = await _jwtService.GenerateAccessTokenFromRefreshToken(tokenDto);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(tokenDto.AccessToken, result.AccessToken);
        Assert.Equal(tokenDto.RefreshToken, result.RefreshToken);
    }

    [Fact]
    public async Task GenerateAccessTokenFromRefreshToken_InvalidRefreshToken_ThrowsException()
    {
        // Arrange
        var user = new AppUser
        {
            UserName = "testuser", RefreshToken = "invalidRefreshToken",
            RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(5)
        };
        var tokenDto = new TokenDto
        {
            AccessToken =
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InRlc3R1c2VyQHRlc3QuY29tIiwicm9sZSI6WyJTdHVkZW50Il19.DV1wVmy27XWi4YLQB2t3_PbL6TzBrqlZfaU4d6VgcTI",
            RefreshToken = "validRefreshToken"
        };

        _userRepositoryMock.Setup(repo => repo.FindByNameAsync("testuser")).ReturnsAsync(user);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _jwtService.GenerateAccessTokenFromRefreshToken(tokenDto));
    }

    [Fact]
    public async Task RevokeRefreshToken_ValidUsername_RevokesToken()
    {
        // Arrange
        var user = new AppUser
        {
            UserName = "testuser", RefreshToken = "validRefreshToken",
            RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(5)
        };

        _userRepositoryMock.Setup(repo => repo.FindByNameAsync("testuser")).ReturnsAsync(user);
        _userRepositoryMock.Setup(repo => repo.UpdateAsync(user)).Returns(Task.CompletedTask);

        // Act
        var result = await _jwtService.RevokeRefreshToken("testuser");

        // Assert
        Assert.True(result);
        Assert.Null(user.RefreshToken);
        Assert.Null(user.RefreshTokenExpiryTime);
    }

    [Fact]
    public async Task RevokeRefreshToken_InvalidUsername_ReturnsFalse()
    {
        // Arrange
        _userRepositoryMock.Setup(repo => repo.FindByNameAsync("invaliduser")).ReturnsAsync((AppUser)null);

        // Act
        var result = await _jwtService.RevokeRefreshToken("invaliduser");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GenerateJwtToken_ValidUser_ReturnsToken()
    {
        // Arrange
        var user = new AppUser { UserName = "testuser" };

        _userRepositoryMock.Setup(repo => repo.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Student" });
        _configurationMock.Setup(config => config.GetSection("JWT:Secret").Value).Returns("supersecretkey123supersecretkey123");
        _configurationMock.Setup(config => config.GetSection("JWT:Issuer").Value).Returns("issuer");
        _configurationMock.Setup(config => config.GetSection("JWT:Audience").Value).Returns("audience");

        // Act
        var result = await _jwtService.GenerateJwtToken(user);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<string>(result);
    }

    [Fact]
    public void GenerateRefreshToken_ReturnsToken()
    {
        // Act
        var result = _jwtService.GenerateRefreshToken();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<string>(result);
    }
}