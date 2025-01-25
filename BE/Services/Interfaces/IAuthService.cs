using BE.DTOs.Auth;
using BE.DTOs.JWT;

namespace BE.Services.Interfaces;

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
}