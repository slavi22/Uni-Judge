using BE.DTOs.Judge;
using BE.Repositories;
using Microsoft.AspNetCore.Mvc;

// TODO: Add tests
// TODO: Add endpoints documentation
namespace BE.Controllers
{
    [ApiController]
    [Route("api/submissions")]
    public class SubmissionController : ControllerBase
    {
        private readonly IJudgeService _judgeService;

        public SubmissionController(IJudgeService judgeService)
        {
            _judgeService = judgeService;
        }

        [HttpPost("createSubmissionBatch")]
        public async Task<IActionResult> CreateSubmissionBatch(SubmissionBatchDto submissions)
        {
            // TODO: Make a custom httpclient that will send requests to the judge BE
            await _judgeService.AddBatchSubmissions(submissions);
            return Ok();
        }
    }
}
