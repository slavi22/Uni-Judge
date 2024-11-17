using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BE.Exceptions;
using BE.Models.Auth;
using BE.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace BE.Services;

// TODO: UPGRADE SOME NUGET PACKAGES IN 'BE.csproj' IN THE FUTURE BECAUSE AS OF NOW MIGRATIONS DONT WORK BECAUSE OF THE POSTGRE'S VERSION (upgrade that first)
public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IJwtService _jwtService;

    public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
        IConfiguration configuration, IJwtService jwtService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _jwtService = jwtService;
    }

    public async Task<TokenModel> LoginUser(LoginModel model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
        if (result.Succeeded)
        {
            var user = await _signInManager.UserManager.FindByNameAsync(model.Email);
            var token = await _jwtService.GenerateJwtToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(30);
            await _userManager.UpdateAsync(user);
            var returnToken = new TokenModel { AccessToken = token, RefreshToken = refreshToken };
            return returnToken;
        }

        return null;
    }

    public async Task<bool> RegisterUser(RegisterModel model)
    {
        var user = new AppUser { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (_userManager.Users.Count() <= 1)
        {
            await _userManager.AddToRolesAsync(user, new string[] { "Student", "Teacher", "Admin" });
        }
        else
        {
            await _userManager.AddToRoleAsync(user, "Student");
        }

        return result.Succeeded;
    }

    public async Task<bool> RegisterTeacher(RegisterTeacherModel model)
    {
        var secret = _configuration.GetSection("TeacherSecret").Value;
        if (model.Secret != secret)
        {
            throw new IncorrectTeacherSecretException("The teacher secret is incorrect.");
        }

        var user = new AppUser { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);
        await _userManager.AddToRolesAsync(user, new string[] { "Student", "Teacher" });
        return result.Succeeded;
    }
}