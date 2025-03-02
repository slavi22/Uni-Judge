using BE.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BE.Presentation.Controllers
{
    [ApiController]
    [Route("api/auth-providers")]
    public class AuthProvidersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthProvidersService _authProvidersService;

        public AuthProvidersController(IConfiguration configuration, IAuthProvidersService authProvidersService)
        {
            _configuration = configuration;
            _authProvidersService = authProvidersService;
        }

        // Google

        // This works with normal "implicit" flow, and the default google button when using the <GoogleLogin /> component from react-google-login
        /*[HttpPost("/signin-oidc")]
        public async Task<IActionResult> SignInGoogle([FromForm] string credential)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(credential);
            //var result = await _userRepository.FindByNameAsync(payload.Email);
            return Ok(payload);
            //return Redirect("http://localhost:5173");
        }*/

        // flow: "auth-code" to be able to have custom google button on the frontend

        /// <summary>
        /// Signs in a user with credentials provided from a Google JWT. If the user doesn't exist, a new user is created from the same JWT.
        /// </summary>
        /// <param name="code">The code the endpoint receives after the user authenticates via Google. That code is then used for a JWT token exchange.</param>
        /// <returns>Redirects to user back to the frontend with set httponly cookies representing the access and refresh token.</returns>
        /// <response code="301">Redirects with http status code <c>301</c> - indicating permanent redirection.</response>
        [HttpGet("signin-google")]
        public async Task<IActionResult> SignInGoogle(string code) // could also specify [FromQuery] string code
        {
            var payload = await _authProvidersService.GetGoogleJwtToken(code);
            var token = await _authProvidersService.SignInGoogle(payload.Email);
            _authProvidersService.SetTokensInsideCookie(token, HttpContext);
            return RedirectPermanent(_configuration.GetSection("Addresses:FE").Value + "?redirected=true");
        }

    }
}