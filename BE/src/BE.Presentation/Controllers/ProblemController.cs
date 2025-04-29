using BE.Business.Services.Interfaces;
using BE.DTOs.DTOs.Problem.Requests;
using BE.DTOs.DTOs.Problem.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Presentation.Controllers
{
    [ApiController]
    [Route("api/problems")]
    public class ProblemController : ControllerBase
    {
        private readonly IProblemService _problemService;

        public ProblemController(IProblemService problemService)
        {
            _problemService = problemService;
        }

        /// <summary>
        /// Create a new problem for a specific course
        /// </summary>
        /// <remarks>Normal users won't be allowed to submit new problems. A person must have the role "Teacher" to be able to create a problem</remarks>
        /// <param name="problem">Contains the new problem details which should be inserted</param>
        /// <returns>Returns a result with the created problem details</returns>
        /// <response code="401">Returns 401 if the user is not authenticated</response>
        /// <response code="403">Returns 403 if the user attempting to create a new problem does not have the "Teacher" role</response>
        /// <response code="200">Returns 200 with the created problem details</response>
        [Authorize(Roles = "Teacher")]
        [HttpPost("create-problem")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(CreatedProblemDto))]
        public async Task<IActionResult> CreateProblem(ClientProblemDto problem)
        {
            var result = await _problemService.CreateProblemAsync(problem);
            return Ok(result);
        }

        //TODO: add test
        /// <summary>
        /// Get all problems created by the authenticated teacher
        /// </summary>
        /// <remarks>Only users with the "Teacher" role can access this endpoint. It returns all problems created by the currently authenticated teacher.</remarks>
        /// <returns>A list of problems created by the teacher with basic details including course ID, problem ID, name, and description</returns>
        /// <response code="401">Returns 401 if the user is not authenticated</response>
        /// <response code="403">Returns 403 if the user attempting to access problems does not have the "Teacher" role</response>
        /// <response code="200">Returns 200 with the list of problems created by the teacher</response>
        [Authorize(Roles = "Teacher")]
        [HttpGet("get-my-problems")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<TeacherProblemsDto>))]
        public async Task<IActionResult> GetMyCreatedProblems()
        {
            var result = await _problemService.GeyMyCreatedProblemsAsync();
            return Ok(result);
        }
    }
}
