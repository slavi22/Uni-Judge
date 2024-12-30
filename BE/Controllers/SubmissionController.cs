using BE.DTOs.Judge.Requests;
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
        [Consumes("application/json")]
        // use [Produces] attribute to specify the response type
        public async Task<IActionResult> CreateSubmissionBatch(BatchSubmissionDto batchSubmissions)
        {
            // TODO: fetch the expected output from the database based on the problem id, then iterate over the list items and set the correct expected output
            var result = await _judgeService.AddBatchSubmissions(batchSubmissions);
            return Ok(result);
        }
    }
}