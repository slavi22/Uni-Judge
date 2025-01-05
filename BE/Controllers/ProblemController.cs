using BE.DTOs.Problem;
using BE.Services.Interfaces;
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


        [HttpPost("createProblem")]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateProblem(CreateProblemDto problem)
        {
            var result = await _problemService.CreateProblem(problem);
            return Ok(result);
        }
    }
}
