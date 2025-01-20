using BE.DTOs.Course;
using BE.DTOs.Course.Requests;
using BE.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
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

        [Authorize(Policy = "HasSignedUpForCourse")]
        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> ViewCourse(string courseId)
        {
            var course = await _courseService.GetProblemsForCourse(courseId);
            return Ok(course);
        }

        [Authorize]
        [HttpPost("signUpForCourse")]
        public async Task<IActionResult> SignUpForCourse(SignUpForCourseDto courseId)
        {
            var result = await _courseService.SignUpForCourse(courseId);
            if (result)
            {
                return Ok("Successfully signed up for course");
            }
            return BadRequest("Failed to sign up for course");
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost("createNewCourse")]
        public async Task<IActionResult> CreateCourse(CreateCourseDto dto)
        {
            await _courseService.CreateNewCourse(dto);
            return Ok();
        }
    }
}
