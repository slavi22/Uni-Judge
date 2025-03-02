using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BE.Business.Services.Interfaces;
using BE.DataAccess.Repositories.Interfaces;
using BE.DTOs.DTOs.JWT.Responses;
using BE.Models.Models.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BE.Business.Services.Implementations;

// Implementation from various sources =>
// https://www.youtube.com/watch?v=6DWJIyipxzw&
// https://dotnetfullstackdev.medium.com/jwt-token-authentication-in-c-a-beginners-guide-with-code-snippets-7545f4c7c597
// https://www.telerik.com/blogs/asp-net-core-basics-authentication-authorization-jwt
public class JwtService : IJwtService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public JwtService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<TokenDto> GenerateAccessTokenFromRefreshToken(TokenDto dto)
    {
        // Here we generate a new access token from the refresh token
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(dto.AccessToken);
        // We extract the username from the token
        var userName = token.Claims.FirstOrDefault(claim => claim.Type == "unique_name")?.Value;
        // We try to find the user in the database
        var user = await _userRepository.FindByNameAsync(userName);
        // If the user is not found or the refresh token is invalid/expired, we throw an exception
        if (user == null || user.RefreshToken != dto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            //TODO: make a custom exception maybe?
            throw new Exception();
        }

        // If the refresh token is valid, we generate a new access token
        var newAccessToken = await GenerateJwtToken(user);
        var newTokenDto = new TokenDto { AccessToken = newAccessToken, RefreshToken = user.RefreshToken };
        return newTokenDto;
    }

    public async Task<bool> RevokeRefreshToken(string username)
    {
        // First we try to find the user in the database by the given username if user is found we set the refresh token and expiry time to null
        var user = await _userRepository.FindByNameAsync(username);
        if (user == null)
        {
            return false;
        }

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;
        await _userRepository.UpdateAsync(user);
        return true;
    }

    public async Task<string> GenerateJwtToken(AppUser user)
    {
        // Here we generate a new JWT token for the user with the given claims which are the user's username and roles
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JWT:Secret").Value);
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = await GenerateClaims(user),
            Expires = DateTime.UtcNow.AddMinutes(60), //TODO: 60 mins is for debugging only
            Issuer = _configuration.GetSection("JWT:Issuer").Value,
            Audience = _configuration.GetSection("JWT:Audience").Value,
            SigningCredentials = credentials,
        };

        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        // Here we generate a new refresh token for the user by generating a random number and converting it to a base64 string
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private async Task<ClaimsIdentity> GenerateClaims(AppUser user)
    {
        // Here we generate the claims for the user which are the user's username and roles which we then add to the claims identity
        var claims = new ClaimsIdentity();
        claims.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
        var roles = await _userRepository.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }
}