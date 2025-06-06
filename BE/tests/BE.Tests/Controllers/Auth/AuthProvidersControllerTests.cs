using BE.Business.Services.Interfaces;
using BE.DTOs.DTOs.JWT.Responses;
using BE.Presentation.Controllers;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace BE.Tests.Controllers.Auth;

public class AuthProvidersControllerTests
{
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<IAuthProvidersService> _authProvidersServiceMock;
    private readonly AuthProvidersController _controller;

    public AuthProvidersControllerTests()
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
        _authProvidersServiceMock.Setup(service => service.GetGoogleJwtToken(code)).ReturnsAsync(payload);
        _authProvidersServiceMock.Setup(service => service.SignInGoogle(email)).ReturnsAsync(It.IsAny<TokenDto>());
        _authProvidersServiceMock.Setup(service => service.SetTokensInsideCookie(It.IsAny<TokenDto>(), It.IsAny<HttpContext>()));
        _configurationMock.Setup(config => config.GetSection("Addresses:FE").Value).Returns("http://localhost:5173");

        // Act
        var result = await _controller.SignInGoogle(code);

        // Assert
        var redirectResult = Assert.IsType<RedirectResult>(result);
        Assert.True(redirectResult.Permanent);
    }

    [Fact]
    public async Task SignInGoogle_ReturnsNull_WhenCodeIsInvalid()
    {
        // Arrange
        var code = "invalidCode";
        _authProvidersServiceMock.Setup(service => service.GetGoogleJwtToken(code))
            .ThrowsAsync(new Exception("Invalid code"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _controller.SignInGoogle(code));
    }
}