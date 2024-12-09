using System.Security.Claims;
using BE.DTOs.Auth;
using BE.DTOs.JWT;
using BE.Models.Auth;

namespace BE.Repositories;

public interface IAuthService
{
    Task<TokenDto> LoginUser(LoginDto dto);
    Task<bool> RegisterUser(RegisterDto dto);
    Task<bool> RegisterTeacher(RegisterTeacherDto dto);
}