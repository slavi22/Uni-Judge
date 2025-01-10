using BE.DTOs.Problem;
using BE.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
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
        /// //TODO: add summary
        /// </summary>
        /// <param name="problem"></param>
        /// <returns></returns>
        /// <response code="403">Returns 403 if the user attempting to create a new problem does not have the "Teacher" role</response>
        //[Authorize(Roles = "Teacher")]
        [HttpPost("createProblem")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Produces("application/json")]
        public async Task<IActionResult> CreateProblem(CreateProblemDto problem)
        {
            var result = await _problemService.CreateProblem(problem);
            return Ok(result);
        }
    }
}
