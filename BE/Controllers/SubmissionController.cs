using BE.DTOs.Judge.Requests;
using BE.Services.Interfaces;
using BE.Util.ExtensionMethods;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUserSubmissionService _userSubmissionService;

        public SubmissionController(IJudgeService judgeService, IUserSubmissionService userSubmissionService)
        {
            _judgeService = judgeService;
            _userSubmissionService = userSubmissionService;
        }

        [Authorize]
        [HttpPost("createSubmission")]
        [Consumes("application/json")]
        [Produces("application/json")]
        // use [Produces] attribute to specify the response type
        // throws 404 if problem or course not found
        public async Task<IActionResult> CreateSubmissionBatch(ClientSubmissionDto clientSubmissionDto)
        {
            var clientSubmissionDtoCopy = clientSubmissionDto.DeepCopyClientSubmissionDto();
            var judgeResults = await _judgeService.AddBatchSubmissions(clientSubmissionDto);
            var result = await _userSubmissionService.AddUserSubmission(clientSubmissionDtoCopy, judgeResults);
            return Ok(result);
        }
    }
}