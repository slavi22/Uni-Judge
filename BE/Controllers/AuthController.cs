using BE.DTOs.Auth;
using BE.DTOs.JWT;
using BE.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Logs in a user with the provided credentials
        /// </summary>
        /// <param name="dto">DTO containing the login credentials</param>
        /// <returns>A DTO containing the created user submission</returns>
        /// <response code="404">Returns 404 if a user isn't found or the provided credentials are incorrect</response>
        /// <response code="200">Returns 200 with the generated JWT and Refresh token</response>
        [HttpPost("login")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound, type: typeof(ProblemDetails))]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(TokenDto))]
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

        /// <summary>
        /// Registers a user with the provided credentials
        /// </summary>
        /// <param name="dto">DTO containing the register credentials</param>
        /// <returns>Returns a response message indicating whether the registration was successful</returns>
        /// <response code="400">Returns a problem detail with status code 400 if the registration wasn't successful</response>
        /// <response code="201">Returns 201 if the user was registered successfully</response>
        [HttpPost("register")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status201Created)]
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

        /// <summary>
        /// Registers a teacher with the provided credentials and secret
        /// </summary>
        /// <param name="dto">DTO containing the teacher registration credentials</param>
        /// <returns>Returns a response message indicating whether the registration was successful</returns>
        /// <response code="400">Returns a problem detail with status code 400 if the teacher registration wasn't successful</response>
        /// <response code="201">Returns 201 if the teacher was registered successfully</response>
        [HttpPost("registerTeacher")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> RegisterTeacher(RegisterTeacherDto dto)
        {
            var result = await _authService.RegisterTeacher(dto);
            if (result == false)
            {
                return Problem(detail: "Teacher registration not successful.", statusCode: StatusCodes.Status400BadRequest,
                    title: "Teacher not registered.");
            }
            return StatusCode(StatusCodes.Status201Created, "Teacher registered successfully.");
        }

        /// <summary>
        /// Refreshes the JWT token of a user with the provided refresh token
        /// </summary>
        /// <param name="dto">DTO containing the access and refresh tokens</param>
        /// <returns>The newly generated token</returns>
        /// <response code="200">Returns 200 with the newly generated token</response>
        //https://www.c-sharpcorner.com/article/jwt-authentication-with-refresh-tokens-in-net-6-0/
        [HttpPost("refreshToken")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(TokenDto))]
        public async Task<IActionResult> RefreshToken(TokenDto dto)
        {
            var newToken = await _jwtService.GenerateAccessTokenFromRefreshToken(dto);
            return Ok(newToken);
        }

        /// <summary>
        /// Revokes the refresh token of a user
        /// </summary>
        /// <param name="username">The username whose token will be revoked</param>
        /// <returns>Returns 200 indicating the successful revoking of the token</returns>
        /// <response code="404">Returns 404 if the user isn't found</response>
        /// <response code="200">Returns 200 if the token was revoked successfully</response>
        [HttpPatch("revoke/{username}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound, type: typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status200OK)]
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