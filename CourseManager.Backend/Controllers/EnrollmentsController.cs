using CourseManager.Application.Features.User;
using CourseManager.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CourseManager.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEnrollmentById(string id)
        {
            var result = await _enrollmentService.GetEnrollmentById(id);
            if (!result.IsSuccessful)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetEnrollmentsByCourseId(string courseId)
        {
            var result = await _enrollmentService.GetEnrollmentsByCourseId(courseId);
            if (!result.IsSuccessful)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEnrollment([FromBody] EnrollmentModel enrollment)
        {
            var result = await _enrollmentService.CreateEnrollment(enrollment);
            if (!result.IsSuccessful)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrollment(string id)
        {
            var result = await _enrollmentService.DeleteEnrollment(id);
            if (!result.IsSuccessful)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpGet("exists")]
        public async Task<IActionResult> CheckIfEnrollmentExists([FromQuery] string courseId, [FromQuery] string email)
        {
            var exists = await _enrollmentService.CheckIfEnrollmentExists(courseId, email);
            return Ok(exists);
        }

        [HttpGet("course/{courseId}/email/{email}")]
        public async Task<IActionResult> GetEnrollment(string courseId, string email)
        {
            var result = await _enrollmentService.GetEnrollment(courseId, email);
            if (!result.IsSuccessful)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Data);
        }
    }
}