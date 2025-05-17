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

        //TODO: add test
        /// <summary>
        /// Gets detailed information about a specific problem by its ID
        /// </summary>
        /// <remarks>Authenticated users can retrieve detailed information about a problem using its unique identifier</remarks>
        /// <param name="courseId">The course identifier the problem belongs to</param>
        /// <param name="problemId">The unique identifier of the problem to retrieve</param>
        /// <returns>Returns detailed information about the requested problem</returns>
        [Authorize(Policy = "HasSignedUpForCourse")]
        [HttpGet("get-problem-info/{courseId}/{problemId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(ProblemInfoDto))]
        public async Task<IActionResult> GetProblemInfo(string courseId, string problemId)
        {
            var result = await _problemService.GetProblemInfoAsync(courseId, problemId);
            return Ok(result);
        }

        //TODO: add test
        /// <summary>
        /// Gets detailed information about a problem for editing purposes
        /// </summary>
        /// <remarks>Only teachers can access this endpoint to retrieve complete problem details including samples, test cases, and configuration needed for editing</remarks>
        /// <param name="courseId">The course identifier the problem belongs to</param>
        /// <param name="problemId">The unique identifier of the problem to retrieve for editing</param>
        /// <returns>Returns comprehensive problem details including solution templates, test cases, and configuration settings</returns>
        /// <response code="200">Returns 200 with the detailed problem information for editing</response>
        /// <response code="401">Returns 401 if the user is not authenticated</response>
        /// <response code="403">Returns 403 if the user does not have the "Teacher" role</response>
        /// <response code="404">Returns 404 if the problem with the specified ID is not found</response>
        [Authorize(Roles = "Teacher")]
        [HttpGet("get-edit-problem-info/{courseId}/{problemId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(EditProblemInfoDto))]
        public async Task<IActionResult> GetEditProblemInfo(string courseId, string problemId)
        {
            var result = await _problemService.GetEditProblemInfoAsync(courseId, problemId);
            return Ok(result);
        }

        /// <summary>
        /// Edit an existing problem in a specific course
        /// </summary>
        /// <remarks>Only users with the "Teacher" role can edit problems. This endpoint updates all problem details including configuration, test cases, and solution templates.</remarks>
        /// <param name="courseId">The course identifier the problem belongs to</param>
        /// <param name="problemId">The unique identifier of the problem to edit</param>
        /// <param name="dto">Contains the updated problem details to be saved</param>
        /// <returns>Returns a result with the updated problem details</returns>
        /// <response code="200">Returns 200 with the updated problem details</response>
        /// <response code="401">Returns 401 if the user is not authenticated</response>
        /// <response code="403">Returns 403 if the user does not have the "Teacher" role</response>
        /// <response code="404">Returns 404 if the problem with the specified ID is not found</response>
        [Authorize(Roles = "Teacher")]
        [HttpPut("edit-problem/{courseId}/{problemId}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(CreatedProblemDto))]
        public async Task<IActionResult> EditProblem(string courseId, string problemId, ClientProblemDto dto)
        {
            var result = await _problemService.EditProblemAsync(courseId, problemId, dto);
            return Ok(result);
        }
    }
}