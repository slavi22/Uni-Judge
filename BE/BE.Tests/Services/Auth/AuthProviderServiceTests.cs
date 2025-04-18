using BE.Business.Services.Interfaces;
using BE.DTOs.DTOs.JWT.Responses;
using BE.Presentation.Controllers;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace BE.Tests.Services.Auth;

public class AuthProviderServiceTests
{
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<IAuthProvidersService> _authProvidersServiceMock;
    private readonly AuthProvidersController _controller;

    public AuthProviderServiceTests()
    {
        _configurationMock = new Mock<IConfiguration>();
        _authProvidersServiceMock = new Mock<IAuthProvidersService>();
        _controller = new AuthProvidersController(_configurationMock.Object, _authProvidersServiceMock.Object);
    }

    [Fact]
    public async Task SignInGoogle_RedirectsToFrontend_WhenCodeIsValid()
    {
        // Arrange
        var code = "validCode";
        var email = "user@example.com";
        var payload = new GoogleJsonWebSignature.Payload { Email = email };
        var token = new TokenDto { AccessToken = "accessToken", RefreshToken = "refreshToken" };
        _authProvidersServiceMock.Setup(service => service.GetGoogleJwtToken(code)).ReturnsAsync(payload);
        _authProvidersServiceMock.Setup(service => service.SignInGoogle(email)).ReturnsAsync(token);
        _authProvidersServiceMock.Setup(service => service.SetTokensInsideCookie(token, It.IsAny<HttpContext>()));
        _configurationMock.Setup(config => config.GetSection("Addresses:FE").Value).Returns("http://localhost:5173");

        // Act
        var result = await _controller.SignInGoogle(code);

        // Assert
        var redirectResult = Assert.IsType<RedirectResult>(result);
        Assert.True(redirectResult.Permanent);
    }

    [Fact]
    public async Task SignInGoogle_ReturnsBadRequest_WhenCodeIsInvalid()
    {
        // Arrange
        var code = "invalidCode";
        _authProvidersServiceMock.Setup(service => service.GetGoogleJwtToken(code))
            .ThrowsAsync(new Exception("Invalid code"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(async () => await _controller.SignInGoogle(code));
    }

    [Fact]
    public async Task SignInGoogle_CreatesNewUser_WhenUserDoesNotExist()
    {
        // Arrange
        var code = "validCode";
        var email = "newuser@example.com";
        var payload = new GoogleJsonWebSignature.Payload { Email = email };
        var token = new TokenDto { AccessToken = "accessToken", RefreshToken = "refreshToken" };
        _authProvidersServiceMock.Setup(service => service.GetGoogleJwtToken(code)).ReturnsAsync(payload);
        _authProvidersServiceMock.Setup(service => service.SignInGoogle(email)).ReturnsAsync(token);
        _authProvidersServiceMock.Setup(service => service.SetTokensInsideCookie(token, It.IsAny<HttpContext>()));
        _configurationMock.Setup(config => config.GetSection("Addresses:FE").Value).Returns("http://localhost:5173");

        // Act
        var result = await _controller.SignInGoogle(code);

        // Assert
        var redirectResult = Assert.IsType<RedirectResult>(result);
        Assert.True(redirectResult.Permanent);
    }

    [Fact]
    public async Task SignInGoogle_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var code = "invalidCode";
        _authProvidersServiceMock.Setup(service => service.GetGoogleJwtToken(code))
            .ThrowsAsync(new Exception("Invalid code"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(async () => await _controller.SignInGoogle(code));
    }

    [Fact]
    public async Task SignInGoogle_ThrowsException_WhenServiceFails()
    {
        // Arrange
        var code = "validCode";
        _authProvidersServiceMock.Setup(service => service.GetGoogleJwtToken(code))
            .ThrowsAsync(new Exception("Service failure"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _controller.SignInGoogle(code));
    }
}