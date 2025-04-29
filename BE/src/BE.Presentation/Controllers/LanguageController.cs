using BE.Business.Services.Interfaces;
using BE.DTOs.DTOs.Language.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Presentation.Controllers
{
    [Route("api/languages")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly ILanguageService _languageService;

        public LanguageController(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        //TODO: add test
        /// <summary>
        /// Gets all languages available in the system
        /// </summary>
        /// <remarks>Only users with the "Teacher" role can access this endpoint. It returns all programming languages supported by the system for code submission and evaluation.</remarks>
        /// <returns>A list of available programming languages with their details</returns>
        /// <response code="401">Returns 401 if the user is not authenticated</response>
        /// <response code="403">Returns 403 if the user does not have the "Teacher" role</response>
        /// <response code="200">Returns 200 with the list of available programming languages</response>
        [Authorize(Roles = "Teacher")]
        [HttpGet("get-all-languages")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<LanguageDto>))]
        public async Task<IActionResult> GetAllLanguages()
        {
            var result = await _languageService.GetAllLanguagesAsync();
            return Ok(result);
        }
    }
}
