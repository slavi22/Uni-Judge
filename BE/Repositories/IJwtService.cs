using BE.DTOs.JWT;
using BE.Models.Auth;

namespace BE.Repositories;

public interface IJwtService
{
    Task<TokenDto> GenerateAccessTokenFromRefreshToken(TokenDto dto);
    Task<bool> RevokeRefreshToken(string userName);
    Task<string> GenerateJwtToken(AppUser user);
    string GenerateRefreshToken();
}