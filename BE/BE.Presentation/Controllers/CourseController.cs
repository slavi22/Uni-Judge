using BE.Business.Services.Interfaces;
using BE.DTOs.DTOs.Course.Requests;
using BE.DTOs.DTOs.Course.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Presentation.Controllers
{
    [ApiController]
    [Route("api/courses")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        /// <summary>
        /// Gets a course's problems
        /// </summary>
        /// <remarks>Requires the user to be signed up for the course</remarks>
        /// <param name="courseId">The id of the course</param>
        /// <returns>A list of problems for the specified course</returns>
        /// <response code="404">Returns 404 if the course could not be found</response>
        /// <response code="403">Returns 403 if the user is not signed up for the course</response>
        /// <response code="200">Returns 200 with the list of problems for the course</response>
        [Authorize(Policy = "HasSignedUpForCourse")]
        [HttpGet("course/{courseId}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound, type: typeof(ProblemDetails))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type:typeof(ProblemDetails))]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<ViewCourseProblemDto>))]
        public async Task<IActionResult> GetCourseProblems(string courseId)
        {
            var courseProblems = await _courseService.GetProblemsForCourse(courseId);
            if (courseProblems == null)
            {
                return Problem(detail: "The requested course could not be found.",
                    statusCode: 404, title: "Course not found.");
            }

            return Ok(courseProblems);
        }

        /// <summary>
        /// Signs up a user for a course
        /// </summary>
        /// <remarks>Requires the user to be authenticated</remarks>
        /// <param name="courseId">The id of the course</param>
        /// <returns>A response indicating if the course sign up was successful</returns>
        /// <response code="400">Returns a problem detail with status code 400 if the sign up wasn't successful</response>
        /// <response code="401">Returns 401 if the user is not authenticated</response>
        /// <response code="200">Returns 200 if the user was signed up successfully</response>
        [Authorize]
        [HttpPost("signUpForCourse")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(ProblemDetails))]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<IActionResult> SignUpForCourse(SignUpForCourseDto courseId)
        {
            var result = await _courseService.SignUpForCourse(courseId);
            if (result)
            {
                return Ok("Successfully signed up for course");
            }

            return BadRequest("Failed signing up for course");
        }

        /// <summary>
        /// Creates a new course
        /// </summary>
        /// <remarks>Requires the signed-in user to have the "Teacher" role</remarks>
        /// <param name="dto">A dto containing the new course details</param>
        /// <returns>A response indicating if the course creation was successful</returns>
        /// <response code="403">Returns 403 if the user does not have the "Teacher" role</response>
        /// <response code="401">Returns 401 if the user is not authorized to create a course</response>
        /// <response code="200">Returns 200 if the course was created successfully</response>
        [Authorize(Roles = "Teacher")]
        [HttpPost("createNewCourse")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(ProblemDetails))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(ProblemDetails))]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateCourse(CreateCourseDto dto)
        {
            await _courseService.CreateNewCourse(dto);
            return Ok();
        }
    }
}