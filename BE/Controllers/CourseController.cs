using BE.DTOs.Course;
using BE.Services.Interfaces;
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

        [HttpPost("createNewCourse")]
        public async Task<IActionResult> CreateCourse(CreateCourseDto dto)
        {
            await _courseService.CreateNewCourse(dto);
            return Ok();
        }
    }
}
