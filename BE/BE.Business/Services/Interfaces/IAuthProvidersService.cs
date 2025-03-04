using BE.DTOs.DTOs.JWT.Responses;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;

namespace BE.Business.Services.Interfaces;

public interface IAuthProvidersService
{
    /// <summary>
    /// Signs in a user with Google as the provider. If the user doesn't exist, it will create a new user.
    /// </summary>
    /// <param name="email">The email extracted from the Google provided JWT token</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the newly created token in the form of a DTO</returns>
    Task<TokenDto> SignInGoogle(string email);
    /// <summary>
    /// Gets the Google JWT token from the code provided by the Google OAuth2 flow.
    /// </summary>
    /// <param name="code">The code provided from the Google OAuth flow as a query parameter</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated JWT token that is mapped to a <c>Payload</c> class, which we can use to access the different claims.</returns>
    Task<GoogleJsonWebSignature.Payload> GetGoogleJwtToken(string code);
    /// <summary>
    /// Sets the access and refresh tokens inside a http only cookie.
    /// </summary>
    /// <param name="tokenDto">The token dto containing the access and refresh token which will be set as an httponly cookie</param>
    /// <param name="context">The current HttpContext</param>
    void SetTokensInsideCookie(TokenDto tokenDto, HttpContext context);
}