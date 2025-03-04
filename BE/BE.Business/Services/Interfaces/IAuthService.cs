using BE.DTOs.DTOs.Auth.Requests;
using BE.DTOs.DTOs.Auth.Responses;
using BE.DTOs.DTOs.JWT.Responses;
using Microsoft.AspNetCore.Http;

namespace BE.Business.Services.Interfaces;

public interface IAuthService
{
    /// <summary>
    /// Logs in a user with the provided login details.
    /// </summary>
    /// <param name="dto">The login details</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the token DTO</returns>
    Task<TokenDto> LoginUser(LoginDto dto);

    /// <summary>
    /// Registers a new user with the provided registration details.
    /// </summary>
    /// <param name="dto">The registration details</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the registration was successful</returns>
    Task<bool> RegisterUser(RegisterDto dto);

    /// <summary>
    /// Registers a new teacher with the provided registration details.
    /// </summary>
    /// <param name="dto">The registration details for the teacher</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the registration was successful</returns>
    Task<bool> RegisterTeacher(RegisterTeacherDto dto);

    /// <summary>
    /// Sets the access and refresh tokens inside a http only cookie.
    /// </summary>
    /// <param name="tokenDto">The token dto containing the access and refresh token which will be set as an httponly cookie</param>
    /// <param name="context">The current HttpContext</param>
    void SetTokensInsideCookie(TokenDto tokenDto, HttpContext context);

    /// <summary>
    /// Gets the current logged-in user's information.
    /// </summary>
    /// <param name="email">The email obtained from the <c>User.Identity.Name</c> claim</param>
    /// <returns></returns>
    Task<UserInfoDto> GetUserInfo(string email);
}