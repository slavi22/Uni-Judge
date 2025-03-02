using BE.Business.Services.Interfaces;
using BE.Common.Exceptions;
using BE.DataAccess.Repositories.Interfaces;
using BE.DTOs.DTOs.Auth.Requests;
using BE.DTOs.DTOs.Auth.Responses;
using BE.DTOs.DTOs.JWT.Responses;
using BE.Models.Models.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace BE.Business.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IJwtService _jwtService;

    public AuthService(IUserRepository userRepository, IConfiguration configuration, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _jwtService = jwtService;
    }

    public async Task<TokenDto> LoginUser(LoginDto dto)
    {
        // Try to login the user with the given email and password
        var result = await _userRepository.PasswordSignInAsync(dto.Email, dto.Password, false, false);
        if (result)
        {
            // If the user is logged in successfully, generate a new token and refresh token and update the user in the database to reflect the new refresh token
            var user = await _userRepository.FindByNameAsync(dto.Email);
            var accessToken = await _jwtService.GenerateJwtToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); //TODO: For debugging only
            await _userRepository.UpdateAsync(user);
            var returnToken = new TokenDto { AccessToken = accessToken, RefreshToken = refreshToken };
            return returnToken;
        }

        return null;
    }

    public async Task<bool> RegisterUser(RegisterDto dto)
    {
        // Register new user with the given email and password

        var user = new AppUser { UserName = dto.Email, Email = dto.Email };
        var result = await _userRepository.CreateAsync(user, dto.Password);
        // If the user count is less than or equal to 1 that means that the user is the first user to be registered and should be given all roles, essentially he is the admin
        if (result && await _userRepository.GetUserCountAsync() <= 1)
        {
            await _userRepository.AddRolesAsync(user, new string[] { "Student", "Teacher", "Admin" });
        }
        else if (result && await _userRepository.GetUserCountAsync() > 1)
        {
            await _userRepository.AddToRoleAsync(user, "Student");
        }

        return result;
    }

    public async Task<bool> RegisterTeacher(RegisterTeacherDto dto)
    {
        // Register new teacher with the given email, password and secret
        var secret = _configuration.GetSection("TeacherSecret").Value;
        // Check if the secret is correct
        if (dto.Secret != secret)
        {
            throw new IncorrectTeacherSecretException("The teacher secret is incorrect.");
        }

        var user = new AppUser { UserName = dto.Email, Email = dto.Email };
        var result = await _userRepository.CreateAsync(user, dto.Password);
        await _userRepository.AddRolesAsync(user, new string[] { "Student", "Teacher" });
        return result;
    }

    public void SetTokensInsideCookie(TokenDto tokenDto, HttpContext context)
    {
        context.Response.Cookies.Append("accessToken", tokenDto.AccessToken, new CookieOptions
        {
            Expires = DateTime.UtcNow.AddMinutes(60), //TODO: 60 mins is for debugging only
            HttpOnly = true,
            IsEssential = true,
            Secure = true,
            SameSite = SameSiteMode.None
        });
        context.Response.Cookies.Append("refreshToken", tokenDto.RefreshToken, new CookieOptions
        {
            Expires = DateTime.UtcNow.AddDays(7), //TODO: Change that to the corresponding value in the AuthService LoginUser method
            HttpOnly = true,
            IsEssential = true,
            Secure = true,
            SameSite = SameSiteMode.None
        });
    }

    public async Task<UserInfoDto> GetUserInfo(string email)
    {
        var user = await _userRepository.FindByNameAsync(email);
        var roles = await _userRepository.GetRolesAsync(user);
        var userInfo = new UserInfoDto
        {
            Email = user.Email,
            Roles = roles.ToList()
        };
        return userInfo;
    }
}