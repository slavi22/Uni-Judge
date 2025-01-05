using BE.DTOs.Auth;
using BE.DTOs.JWT;

namespace BE.Services.Interfaces;

public interface IAuthService
{
    Task<TokenDto> LoginUser(LoginDto dto);
    Task<bool> RegisterUser(RegisterDto dto);
    Task<bool> RegisterTeacher(RegisterTeacherDto dto);
}