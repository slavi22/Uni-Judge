using BE.DTOs.Judge.Requests;
using BE.Models.Auth;
using BE.Repositories.Implementations;
using BE.Repositories.Interfaces;
using BE.Services.Implementations;
using BE.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly IUserRepository _userRepository;

        public SubmissionController(IJudgeService judgeService, IUserSubmissionService userSubmissionService,
            IUserRepository userRepository)
        {
            _judgeService = judgeService;
            _userSubmissionService = userSubmissionService;
            _userRepository = userRepository;
        }

        [Authorize]
        [HttpPost("createSubmission")]
        [Consumes("application/json")]
        // use [Produces] attribute to specify the response type
        public async Task<IActionResult> CreateSubmissionBatch(ClientSubmissionDto clientSubmissionDto)
        {
            var result = await _judgeService.AddBatchSubmissions(clientSubmissionDto);
            //TODO: Now that we have the result with the tokens and all other things make sure to add the data to the db tables
            await _userSubmissionService.AddUserSubmission(clientSubmissionDto, result);
            //TODO: Send the results (which contains things like required acceptance percentage, current submission acceptance percentage, passed test cases out of total test cases)
            return Ok(result);
        }
    }
}