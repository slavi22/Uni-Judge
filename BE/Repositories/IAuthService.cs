using System.Security.Claims;
using BE.Models.Auth;

namespace BE.Repositories;

public interface IAuthService
{
    Task<TokenModel> LoginUser(LoginModel model);
    Task<bool> RegisterUser(RegisterModel model);
    Task<bool> RegisterTeacher(RegisterTeacherModel model);
}