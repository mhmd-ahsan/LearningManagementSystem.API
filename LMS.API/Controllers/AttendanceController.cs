using LMS.API.Dtos;
using LMS.API.Helpers;
using LMS.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceRepository _attendanceRepository;

        public AttendanceController(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        // ✅ Mark attendance
        [HttpPost("mark")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> MarkAttendance([FromBody] AttendanceCreateDto dto)
        {
            var teacherId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var result = await _attendanceRepository.MarkAttendanceAsync(dto, teacherId);
            if (!result)
            {
                return BadRequest(ResponseHelper<string>.Fail("Failed to mark attendance"));
            }

            return Ok(ResponseHelper<string>.Success(null, "Attendance marked successfully"));
        }

        // ✅ View attendance
        [HttpGet("view")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> ViewAttendance([FromQuery] int courseId, [FromQuery] DateTime date)
        {
            var result = await _attendanceRepository.GetAttendanceByCourseAndDateAsync(courseId, date);
            if (result == null)
            {
                return NotFound(ResponseHelper<string>.Fail("No attendance found for the given course/date"));
            }

            return Ok(ResponseHelper<AttendanceViewDto>.Success(result, "Attendance retrieved successfully"));
        }
    }
}
