using BE.DTOs.Auth;
using BE.DTOs.Judge;
using BE.DTOs.JWT;
using BE.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// TODO: Add tests
// TODO: Add endpoints documentation
namespace BE.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;

        public AuthController(IAuthService authService, IJwtService jwtService)
        {
            _authService = authService;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var loginToken = await _authService.LoginUser(dto);
            if (loginToken == null)
            {
                return Problem(detail: "The requested user could not be found.",
                    statusCode: StatusCodes.Status404NotFound, title: "User not found.");
            }

            return Ok(loginToken);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.RegisterUser(dto);
            if (result == false)
            {
                return Problem(detail: "User registration not successful.", statusCode: StatusCodes.Status400BadRequest,
                    title: "User not registered.");
            }

            return StatusCode(StatusCodes.Status201Created, "User registered successfully.");
        }

        [HttpPost("registerTeacher")]
        public async Task<IActionResult> RegisterTeacher(RegisterTeacherDto dto)
        {
            await _authService.RegisterTeacher(dto);
            return StatusCode(StatusCodes.Status201Created, "User registered successfully.");
        }

        //https://www.c-sharpcorner.com/article/jwt-authentication-with-refresh-tokens-in-net-6-0/
        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken(TokenDto dto)
        {
            var newToken = await _jwtService.GenerateAccessTokenFromRefreshToken(dto);
            return Ok(newToken);
        }

        [HttpPatch("revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            var result = await _jwtService.RevokeRefreshToken(username);
            if (result == false)
            {
                return Problem(detail: "The requested user could not be found.",
                    statusCode: StatusCodes.Status404NotFound, title: "User not found.");
            }

            return Ok();
        }
    }
}