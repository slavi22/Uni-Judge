using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BE.DTOs.JWT;
using BE.Models.Auth;
using BE.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace BE.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<AppUser> _userManager;

    public JwtService(IConfiguration configuration, UserManager<AppUser> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task<TokenDto> GenerateAccessTokenFromRefreshToken(TokenDto dto)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(dto.AccessToken);
        var userName = token.Claims.FirstOrDefault(claim => claim.Type == "unique_name")?.Value;
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null || user.RefreshToken != dto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new Exception();
        }

        var newAccessToken = await GenerateJwtToken(user);
        var newTokenModel = new TokenDto { AccessToken = newAccessToken, RefreshToken = user.RefreshToken };
        return newTokenModel;
    }

    public async Task<bool> RevokeRefreshToken(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
        {
            return false;
        }

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;
        await _userManager.UpdateAsync(user);
        return true;
    }

    public async Task<string> GenerateJwtToken(AppUser user)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JWT:Secret").Value);
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = await GenerateClaims(user),
            Expires = DateTime.UtcNow.AddMinutes(1),
            Issuer = _configuration.GetSection("JWT:Issuer").Value,
            Audience = _configuration.GetSection("JWT:Audience").Value,
            SigningCredentials = credentials,
        };

        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private async Task<ClaimsIdentity> GenerateClaims(AppUser user)
    {
        var claims = new ClaimsIdentity();
        claims.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }
}