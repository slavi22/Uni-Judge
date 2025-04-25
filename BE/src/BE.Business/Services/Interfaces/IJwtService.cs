using BE.DTOs.DTOs.JWT.Responses;
using BE.Models.Models.Auth;

namespace BE.Business.Services.Interfaces;

public interface IJwtService
{
    /// <summary>
    /// Generates a new access token using the provided refresh token.
    /// </summary>
    /// <param name="dto">The token DTO containing the refresh token</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the new token DTO</returns>
    Task<TokenDto> GenerateAccessTokenFromRefreshToken(TokenDto dto);

    /// <summary>
    /// Revokes the refresh token for the specified user.
    /// </summary>
    /// <param name="username">The username of the user whose refresh token is to be revoked</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the revocation was successful</returns>
    Task<bool> RevokeRefreshToken(string username);

    /// <summary>
    /// Generates a new JWT token for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to generate the JWT token</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the generated JWT token as a string</returns>
    Task<string> GenerateJwtToken(AppUser user);

    /// <summary>
    /// Generates a new refresh token.
    /// </summary>
    /// <returns>The generated refresh token as a string</returns>
    string GenerateRefreshToken();
}