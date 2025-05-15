using BE.Business.Services.Interfaces;
using BE.DTOs.DTOs.Judge.Requests;
using BE.DTOs.DTOs.Judge.Responses;
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
        [Authorize(Policy = "HasSignedUpForCourse")]
        [HttpPost("create-submission/{courseId}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound, type: typeof(ProblemDetails))]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(UserSubmissionResultDto))]
        public async Task<IActionResult> CreateSubmissionBatch(ClientSubmissionDto clientSubmissionDto)
        {
            var clientSubmissionDtoCopy = clientSubmissionDto.DeepCopyClientSubmissionDto();
            var judgeResults = await _judgeService.CreateBatchSubmissions(clientSubmissionDto);
            var result = await _userSubmissionService.AddUserSubmission(clientSubmissionDtoCopy, judgeResults);
            return Ok(result);
        }

        /// <summary>
        /// Tests a submission batch for a specific problem without saving to database
        /// </summary>
        /// <remarks>Requires an authenticated user to access</remarks>
        /// <param name="clientSubmissionTestDto">The client submission DTO containing the submission details</param>
        /// <returns>A DTO containing the judge results for the test submission</returns>
        /// <response code="404">Returns 404 if a problem or a course isn't found</response>
        /// <response code="200">Returns 200 with the judge results for the test submission</response>
        // TODO: add test
        [Authorize(Policy = "HasSignedUpForCourse")]
        [HttpPost("test-submission/{courseId}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound, type: typeof(ProblemDetails))]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK,
            type: typeof(List<TestSubmissionBatchResultResponseDto>))]
        public async Task<IActionResult> TestSubmissionBatch(ClientSubmissionTestDto clientSubmissionTestDto)
        {
            var clientSubmissionDtoCopy = clientSubmissionTestDto.DeepCopyClientSubmissionTestDto();
            var judgeResults = await _judgeService.TestBatchSubmissions(clientSubmissionDtoCopy);
            return Ok(judgeResults);
        }

        /// <summary>
        /// Retrieves all submissions made by the authenticated user for a specific problem in a course
        /// </summary>
        /// <remarks>Requires an authenticated user to access who has signed up for the course</remarks>
        /// <param name="courseId">The unique identifier of the course</param>
        /// <param name="problemId">The unique identifier of the problem</param>
        /// <returns>A list of the user's submissions for the specified problem</returns>
        /// <response code="200">Returns 200 with the list of user's submissions</response>
        /// <response code="404">Returns 404 if the course or problem is not found</response>
        /// <response code="401">Returns 401 if the user is not authenticated</response>
        /// <response code="403">Returns 403 if the user has not signed up for the course</response>
        [Authorize(Policy = "HasSignedUpForCourse")]
        [HttpGet("get-problem-submissions/{courseId}/{problemId}")]
        [Produces("application/json")]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound, type: typeof(ProblemDetails))]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<ProblemUserSubmissionsDto>))]
        public async Task<IActionResult> GetProblemSubmissions(string courseId, string problemId)
        {
            var result = await _userSubmissionService.GetUserSubmissionsForSpecificProblem(courseId, problemId);
            return Ok(result);
        }
    }
}