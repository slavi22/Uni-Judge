using BE.Business.Services.Interfaces;
using BE.DataAccess.Repositories.Interfaces;
using BE.DTOs.DTOs.JWT.Responses;
using BE.Models.Models.Auth;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace BE.Business.Services.Implementations;

public class AuthProvidersService : IAuthProvidersService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IConfiguration _configuration;

    public AuthProvidersService(IUserRepository userRepository, IJwtService jwtService, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _configuration = configuration;
    }

    public async Task<TokenDto> SignInGoogle(string email)
    {
        var user = await _userRepository.FindByNameAsync(email);
        if (user == null)
        {
            return await CreateNewUserWithGoogleProvider(email);
        }
        return await LoginExistingGoogleUser(user);
    }

    private async Task<TokenDto> LoginExistingGoogleUser(AppUser user)
    {
        // If the user exists, generate a new token and refresh token and update the user in the database to reflect the new refresh token
        var accessToken = await _jwtService.GenerateJwtToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); //TODO: For debugging only
        await _userRepository.UpdateAsync(user);

        var token = new TokenDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
        return token;
    }

    private async Task<TokenDto> CreateNewUserWithGoogleProvider(string email)
    {
        // Creates and registers a new user with the provided email
        // Since we are using Google as the provider we need about the password (it will be null in the db)
        var newUser = new AppUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };
        var accessToken = await _jwtService.GenerateJwtToken(newUser);
        var refreshToken = _jwtService.GenerateRefreshToken();
        newUser.RefreshToken = refreshToken;
        newUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); //TODO: For debugging only
        await _userRepository.CreateAsync(newUser, null!);
        await _userRepository.AddToRoleAsync(newUser, "Student");

        var token = new TokenDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
        return token;
    }

    public async Task<GoogleJsonWebSignature.Payload> GetGoogleJwtToken(string code)
    {
        // mostly got it working from here and a little help from chatgpt => https://github.com/googleapis/google-api-dotnet-client/issues/1486
        // which i found from this thread => https://github.com/MomenSherif/react-oauth/issues/12#issuecomment-2248557741
        // => https://github.com/googleapis/google-api-dotnet-client/issues/1486#issuecomment-566993416
        // => from this it shows creating the GoogleAuthorizationCodeFlow with client secrets
        // then in the if (tokenResponse == null), it shows how to exchange the code for a token
        // after that in here => https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-9.0#sign-in-with-google
        // the last line it tells us to do this => GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(credential);
        // for us to get the final payload which includes the email and other information

        // some reference from here as well
        // => https://code-maze.com/how-to-sign-in-with-google-angular-aspnet-webapi/#:~:text=After%20that%2C%20we%20can%20add%20a%20new%20method%20to%20validate%20idtoken%3A
        var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = _configuration.GetSection("Authentication:Google:ClientId").Value,
                ClientSecret = _configuration.GetSection("Authentication:Google:ClientSecret").Value
            }
        });
        // here we don't care about the first parameter - userId, since we only want to exchange the code for a JWT token
        var token = await flow.ExchangeCodeForTokenAsync("", code,
            _configuration.GetSection("Addresses:GoogleRedirectUri").Value, CancellationToken.None);
        var payload = await GoogleJsonWebSignature.ValidateAsync(token.IdToken);
        return payload;
    }

    public void SetTokensInsideCookie(TokenDto tokenDto, HttpContext context)
    {
        context.Response.Cookies.Append("accessToken", tokenDto.AccessToken, new CookieOptions
        {
            Expires = DateTime.UtcNow.AddMinutes(60), //TODO: 60 mins is for debugging only
            HttpOnly = true,
            IsEssential = true,
            Secure = true,
            SameSite = SameSiteMode.None // TODO: When deploying check to see if either Lax or Strict will work (since the be and fe will be on different domains)
        });
        context.Response.Cookies.Append("refreshToken", tokenDto.RefreshToken, new CookieOptions
        {
            Expires = DateTime.UtcNow
                .AddDays(7), //TODO: Change that to the corresponding value in the AuthService LoginUser method
            HttpOnly = true,
            IsEssential = true,
            Secure = true,
            SameSite = SameSiteMode.None // TODO: When deploying check to see if either Lax or Strict will work (since the be and fe will be on different domains)
        });
    }
}