using BE.Models.Auth;
using BE.Repositories;
using Microsoft.AspNetCore.Mvc;

// TODO: Add tests
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
        public async Task<IActionResult> Login(LoginModel model)
        {
            var loginToken = await _authService.LoginUser(model);
            if (loginToken == null)
            {
                return Problem(detail: "The requested user could not be found.",
                    statusCode: StatusCodes.Status404NotFound, title: "User not found.");
            }

            return Ok(loginToken);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var result = await _authService.RegisterUser(model);
            if (result == false)
            {
                return Problem(detail : "User registration not successful.", statusCode: StatusCodes.Status400BadRequest, title: "User not registered.");
            }
            return StatusCode(StatusCodes.Status201Created, "User registered successfully.");
        }

        [HttpPost("registerTeacher")]
        public async Task<IActionResult> RegisterTeacher(RegisterTeacherModel model)
        {
            await _authService.RegisterTeacher(model);
            return StatusCode(StatusCodes.Status201Created, "User registered successfully.");
        }

        //https://www.c-sharpcorner.com/article/jwt-authentication-with-refresh-tokens-in-net-6-0/
        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken(TokenModel model)
        {
            var newToken = await _jwtService.GenerateAccessTokenFromRefreshToken(model);
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