using LMS.API.Dtos;
using LMS.API.Helpers;
using LMS.API.Models;
using LMS.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Teacher")] // Only teachers can upload
    public class LessonController : ControllerBase
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LessonController(ILessonRepository lessonRepository, IWebHostEnvironment webHostEnvironment)
        {
            _lessonRepository = lessonRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPatch("upload-lesson")]
        public async Task<IActionResult> UploadLesson([FromForm] LessonCreateDto dto)
        {
            if (dto == null || dto.File == null || dto.File.Length == 0)
            {
                return BadRequest(ResponseHelper<string>.Fail("No file uploaded"));
            }

            // ✅ Extract teacher id from JWT
            var uploadedByClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(uploadedByClaim))
                return Unauthorized(ResponseHelper<string>.Fail("Invalid token"));

            var uploadedBy = int.Parse(uploadedByClaim);

            // ✅ Set upload directory
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath ?? "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // ✅ File details
            var fileExt = Path.GetExtension(dto.File.FileName).ToLower();
            var fileType = fileExt == ".pdf" ? "PDF" : "Video"; // Store type for DB

            var fileName = $"{Guid.NewGuid()}{fileExt}";
            var fullFilePath = Path.Combine(uploadsFolder, fileName);

            // ✅ Save file to server
            using (var stream = new FileStream(fullFilePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            // ✅ Create Lesson object
            var lesson = new Lesson
            {
                CourseId = dto.CourseId,
                Title = dto.Title,
                FilePath = $"/uploads/{fileName}",
                FileType = fileType,
                UploadedBy = uploadedBy,
                UploadedAt = DateTime.UtcNow
            };

            var success = await _lessonRepository.UploadLessonAsync(lesson);
            if (!success)
            {
                return BadRequest(ResponseHelper<string>.Fail("Failed to save lesson"));
            }

            return Ok(ResponseHelper<string>.Success(null, "Lesson uploaded successfully"));
        }

        [HttpGet("course-lessons/{courseId}")]
        public async Task<IActionResult> GetLessonsByCourseId(int courseId)
        {
            var lessons = await _lessonRepository.GetLessonsByCourseIdAsync(courseId);

            if (lessons == null || !lessons.Any())
            {
                return NotFound(ResponseHelper<string>.Fail($"No lessons found for Course ID: {courseId}", 404));
            }

            return Ok(ResponseHelper<IEnumerable<LessonResponseDto>>.Success(lessons, "Lessons fetched successfully"));
        }

    }
}
