using BE.Business.Services.Interfaces;
using BE.DTOs.DTOs.Auth.Requests;
using BE.DTOs.DTOs.Auth.Responses;
using BE.DTOs.DTOs.JWT.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// TODO: Add tests
namespace BE.Presentation.Controllers
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
        /// <response code="200">Returns 200 and sets 2 httponly cookies, one for the access and another for the refresh token</response>
        [HttpPost("login")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound, type: typeof(NotFoundResponse))]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var loginToken = await _authService.LoginUser(dto);
            if (loginToken == null)
            {
                /*return Problem(detail: "The requested user could not be found.",
                    statusCode: StatusCodes.Status404NotFound, title: "User not found.");*/
                return NotFound(new NotFoundResponse
                {
                    Detail = "The requested user could not be found.", Title = "User not found.",
                    Status = StatusCodes.Status404NotFound
                });
            }

            // since im using "AddIdentity" it apparently adds a cookie with the name ".AspNetCore.Identity.Application" so i have to delete the Set-Cookie header
            // otherwise it will get set along with the accessToken and refreshToken cookies
            // https://github.com/openiddict/openiddict-core/issues/578#issuecomment-375818767
            HttpContext.Response.Headers.Remove("Set-Cookie");
            _authService.SetTokensInsideCookie(loginToken, HttpContext);

            return Ok();
        }

        /// <summary>
        /// Registers a user with the provided credentials
        /// </summary>
        /// <param name="dto">DTO containing the register credentials</param>
        /// <returns>Returns a response message indicating whether the registration was successful</returns>
        /// <response code="400">Returns a problem detail with status code 400 if the registration wasn't successfu.</response>
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
        [HttpPost("register-teacher")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> RegisterTeacher(RegisterTeacherDto dto)
        {
            var result = await _authService.RegisterTeacher(dto);
            if (result == false)
            {
                return Problem(detail: "Teacher registration not successful.",
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Teacher not registered.");
            }

            return StatusCode(StatusCodes.Status201Created, "Teacher registered successfully.");
        }

        /// <summary>
        /// Refreshes the JWT token of a user with the provided refresh token
        /// </summary>
        /// <returns>The newly generated token</returns>
        /// <response code="200">Returns 200 with the newly generated token</response>
        /// <response code="401">Returns 401 when the refresh token is missing from the request</response>
        //https://www.c-sharpcorner.com/article/jwt-authentication-with-refresh-tokens-in-net-6-0/
        [HttpPost("refresh-token")]
        [Produces("application/json")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken()
        {
            // TODO: refine the implementation, because it currently needs an access token to be able to refresh the refresh token
            // => https://youtu.be/kR_9gRBeRMQ?si=DOPAFx1ebqfQ2J7-&t=514
            HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken);
            if (refreshToken == null)
            {
                Response.Cookies.Delete("accessToken",
                    new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.None });
                Response.Cookies.Delete("refreshToken",
                    new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.None });
                // TODO: Change this to a more fitting response
                return Unauthorized();
            }

            var tokenDto = new TokenDto { RefreshToken = refreshToken };
            var newToken = await _jwtService.GenerateAccessTokenFromRefreshToken(tokenDto);
            _authService.SetTokensInsideCookie(newToken, HttpContext);
            return Ok();
        }

        /// <summary>
        /// Revokes the refresh token of a user
        /// </summary>
        /// <param name="username">The username whose token will be revoked</param>
        /// <returns>Returns 200 indicating the successful revoking of the token</returns>
        /// <response code="404">Returns 404 if the user isn't found</response>
        /// <response code="200">Returns 200 if the token was revoked successfully</response>
        [HttpPatch("revoke/{username}")]
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

        /// <summary>
        /// Logs out the currently authenticated user
        /// </summary>
        /// <response code="200">Returns 200 indicating the successful logout</response>
        /// <returns>Returns 200 indicating the successful logout</returns>
        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("accessToken",
                new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.None });
            Response.Cookies.Delete("refreshToken",
                new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.None });
            return Ok();
        }

        /// <summary>
        /// Gets the user info of the currently authenticated user.
        /// </summary>
        /// <returns>Returns 200 with the user info.</returns>
        /// <response code="200">Returns 200 with the user's info, if the user is authenticated.</response>
        [Authorize]
        [HttpGet("me")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(UserInfoDto))]
        public async Task<IActionResult> UserInfo()
        {
            var userInfo = await _authService.GetUserInfo(User.Identity.Name);
            return Ok(userInfo);
        }
    }
}