using BE.DTOs.Auth;
using BE.DTOs.JWT;
using BE.Exceptions;
using BE.Models.Auth;
using BE.Repositories.Interfaces;
using BE.Services.Interfaces;

namespace BE.Services.Implementations;

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
        var result = await _userRepository.PasswordSignInAsync(dto.Email, dto.Password, false, false);
        if (result)
        {
            var user = await _userRepository.FindByNameAsync(dto.Email);
            var token = await _jwtService.GenerateJwtToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(30);
            await _userRepository.UpdateAsync(user);
            var returnToken = new TokenDto { AccessToken = token, RefreshToken = refreshToken };
            return returnToken;
        }

        return null;
    }

    public async Task<bool> RegisterUser(RegisterDto dto)
    {
        var user = new AppUser { UserName = dto.Email, Email = dto.Email };
        var result = await _userRepository.CreateAsync(user, dto.Password);
        if (await _userRepository.GetUserCountAsync() <= 1)
        {
            await _userRepository.AddRolesAsync(user, new string[] { "Student", "Teacher", "Admin" });
        }
        else
        {
            await _userRepository.AddToRoleAsync(user, "Student");
        }

        return result;
    }

    public async Task<bool> RegisterTeacher(RegisterTeacherDto dto)
    {
        var secret = _configuration.GetSection("TeacherSecret").Value;
        if (dto.Secret != secret)
        {
            throw new IncorrectTeacherSecretException("The teacher secret is incorrect.");
        }

        var user = new AppUser { UserName = dto.Email, Email = dto.Email };
        var result = await _userRepository.CreateAsync(user, dto.Password);
        await _userRepository.AddRolesAsync(user, new string[] { "Student", "Teacher" });
        return result;
    }
}