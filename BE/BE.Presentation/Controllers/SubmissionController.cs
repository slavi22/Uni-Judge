using BE.Business.Services.Interfaces;
using BE.DTOs.DTOs.Judge.Requests;
using BE.DTOs.DTOs.UserSubmission.Responses;
using BE.Extensions.ExtensionMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Presentation.Controllers
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

        /// <summary>
        /// Creates a new submission batch for a specific problem to be sent to the judge
        /// </summary>
        /// <remarks>Requires an authenticated user to access</remarks>
        /// <param name="clientSubmissionDto">The client submission DTO containing the submission details</param>
        /// <returns>A DTO containing the created user submission</returns>
        /// <response code="404">Returns 404 if a problem or a course isn't found</response>
        /// <response code="200">Returns 200 with the created user submission</response>
        [Authorize]
        [HttpPost("create-submission")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(statusCode:StatusCodes.Status404NotFound, type:typeof(ProblemDetails))]
        [ProducesResponseType(statusCode:StatusCodes.Status200OK, type:typeof(UserSubmissionDto))]
        public async Task<IActionResult> CreateSubmissionBatch(ClientSubmissionDto clientSubmissionDto)
        {
            var clientSubmissionDtoCopy = clientSubmissionDto.DeepCopyClientSubmissionDto();
            var judgeResults = await _judgeService.CreateBatchSubmissions(clientSubmissionDto);
            var result = await _userSubmissionService.AddUserSubmission(clientSubmissionDtoCopy, judgeResults);
            return Ok(result);
        }
    }
}