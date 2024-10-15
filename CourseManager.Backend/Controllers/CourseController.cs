using CourseManager.Application.Features.User;
using CourseManager.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CourseManager.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourses([FromQuery] int curPage, [FromQuery] int pageSize, [FromQuery] string searchString, [FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo)
        {
            var result = await _courseService.GetAllCourses(curPage, pageSize, searchString, dateFrom, dateTo);
            return Ok(result);
        }

        [HttpGet("user/enrolled")]
        public async Task<IActionResult> GetUserEnrolledCourses([FromQuery] string userId, [FromQuery] int curPage, [FromQuery] int pageSize, [FromQuery] string searchString, [FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo)
        {
            var result = await _courseService.GetUserEnrolledCourses(userId, curPage, pageSize, searchString, dateFrom, dateTo);
            return Ok(result);
        }

        [HttpGet("user/not-enrolled")]
        public async Task<IActionResult> GetCoursesUserIsNotEnrolledIn([FromQuery] string userId, [FromQuery] int curPage, [FromQuery] int pageSize, [FromQuery] string searchString, [FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo)
        {
            var result = await _courseService.GetCoursesUserIsNotEnrolledIn(userId, curPage, pageSize, searchString, dateFrom, dateTo);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseByIdForCreateEdit(string id)
        {
            var result = await _courseService.GetCourseByIdForCreateEdit(id);
            if (!result.IsSuccessful)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetCourseDetailsById(string id)
        {
            var result = await _courseService.GetCourseDetailsById(id);
            if (!result.IsSuccessful)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CourseNewEditModel course)
        {
            var result = await _courseService.CreateCourse(course);
            if (!result.IsSuccessful)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(string id, [FromBody] CourseNewEditModel course)
        {
            var result = await _courseService.UpdateCourse(id, course);
            if (!result.IsSuccessful)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(string id)
        {
            var result = await _courseService.DeleteCourse(id);
            if (!result.IsSuccessful)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpGet("capacity/{id}")]
        public async Task<IActionResult> CheckIfCourseCapacityFull(string id)
        {
            var result = await _courseService.CheckIfCourseCapacityFull(id);
            return Ok(result);
        }
    }
}