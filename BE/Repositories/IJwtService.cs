using BE.Models.Auth;

namespace BE.Repositories;

public interface IJwtService
{
    Task<TokenModel> GenerateAccessTokenFromRefreshToken(TokenModel model);
    Task<bool> RevokeRefreshToken(string userName);
    Task<string> GenerateJwtToken(AppUser user);
    string GenerateRefreshToken();
}