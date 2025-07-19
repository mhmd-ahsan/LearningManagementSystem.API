using LMS.API.Dtos;
using LMS.API.Helpers;
using LMS.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _studentrepo;


        public StudentController(IStudentRepository studentrepo)
        {
            _studentrepo = studentrepo;
        }

        [HttpPost("enroll")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> EnrollInCourse([FromBody] StudentCourseEnrollDto dto)
        {
            var studentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            // 🛡️ Override for safety
            dto.StudentId = studentId;

            var success = await _studentrepo.EnrollStudentInCourseAsync(dto);

            if (!success)
            {
                return BadRequest(ResponseHelper<string>.Fail("Enrollment failed"));
            }

            return Ok(ResponseHelper<string>.Success("Enrolled in course successfully"));
        }

        // ✅ Get enrolled courses for logged-in student
        [HttpGet("enrolled-courses")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetEnrolledCourses()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Invalid token.");

            var student = await _studentrepo.GetByUserIdAsync(userId);
            if (student == null)
                return NotFound("Student not found.");

            var courses = await _studentrepo.GetEnrolledCoursesAsync(student.StudentId);
            return Ok(ResponseHelper<IEnumerable<CourseDto>>.Success(courses));
        }

        // ✅ View lessons for enrolled courses
        [HttpGet("lessons")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetLessonsForEnrolledCourses()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Invalid token.");

            var student = await _studentrepo.GetByUserIdAsync(userId);
            if (student == null)
                return NotFound("Student not found.");

            var lessons = await _studentrepo.GetLessonsForStudentAsync(student.StudentId);
            return Ok(ResponseHelper<IEnumerable<LessonDto>>.Success(lessons));
        }

        [HttpPost("submit-quiz")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SubmitQuiz([FromBody] QuizAttemptDto attemptDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Invalid token.");

            var student = await _studentrepo.GetByUserIdAsync(userId);
            if (student == null)
                return NotFound("Student not found.");

            var result = await _studentrepo.SubmitQuizAsync(attemptDto, student.StudentId);
            return Ok(ResponseHelper<QuizResultDto>.Success(result));
        }

        [HttpGet("quiz-questions/{quizId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetQuizQuestions(int quizId)
        {
            var questions = await _studentrepo.GetQuizQuestionsAsync(quizId);
            return Ok(ResponseHelper<IEnumerable<QuizQuestionDto>>.Success(questions));
        }

        [HttpGet("attendance")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetAttendanceRecords()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Invalid token.");

            var student = await _studentrepo.GetByUserIdAsync(userId);
            if (student == null)
                return NotFound("Student not found.");

            var attendance = await _studentrepo.GetAttendanceByStudentIdAsync(student.StudentId);
            return Ok(ResponseHelper<IEnumerable<AttendanceDto>>.Success(attendance));
        }

    }
}
  
