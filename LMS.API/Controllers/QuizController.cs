using Google.Protobuf.WellKnownTypes;
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
    public class QuizController : ControllerBase
    {
        private readonly IQuizRepository _quizrepo;

        public QuizController(IQuizRepository quizrepo)
        {
            _quizrepo = quizrepo;
        }


        [HttpPost("create")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> CreateQuiz([FromBody] QuizCreateDto dto)
        {
            int teacherId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);


            var result = await _quizrepo.PostQuizAsync(dto, teacherId);
            if (!result)
            {
                return BadRequest(ResponseHelper<string>.Fail("Failed to create quiz"));
            }

            return Ok(ResponseHelper<string>.Success("Quiz created successfully"));
        }

        [HttpGet("by-course/{courseId}")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetQuizzesByCourse(int courseId)
        {
            var quizes = await _quizrepo.GetQuizzesByCourseIdAsync(courseId);

            if(quizes == null || quizes.Count == 0)
            {
                return NotFound(ResponseHelper<string>.Fail($"No quizes found for this courseId: {courseId}"));
            }

            return Ok(ResponseHelper<List<QuizViewDto>>.Success(quizes,"Quizzes retrieved successfully"));
        }
    }
}
