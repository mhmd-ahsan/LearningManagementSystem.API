using LMS.API.Dtos;
using LMS.API.Helpers;
using LMS.API.Models;
using LMS.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")] //Only Admin Access
    public class CourseController : ControllerBase
    {
      
        private readonly ICourseRepository _courseRepository;

        public CourseController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }


        [HttpGet("get-all-courses")]
        public async Task<IActionResult> GetAllCourses()
        {
            var course = await _courseRepository.GetAllCoursesAsync();
            return Ok(ResponseHelper<IEnumerable<CourseResponseDto>>.Success(course,"All Courses fetched successfully"));
        }

        [HttpGet("course-by-id/{id}")]

        public async Task<IActionResult> GetCourseById( int id)
        {
            var course = await _courseRepository.GetCourseByIdAsync(id);
            if(course == null)
            {
                return NotFound(ResponseHelper<CourseResponseDto>.Fail($"Course not found by this id: {id}"));
            }

            return Ok(ResponseHelper<CourseResponseDto>.Success(course,"Course found"));
        }

        [HttpPost("create-course")]
        public async Task<IActionResult> CreateCourse([FromBody] CourseCreateDto dto)
        {
            var course = await _courseRepository.CreateCourseAsync(dto);

            if (!course)
            {
                return BadRequest(ResponseHelper<CourseCreateDto>.Fail("Failed to create course"));
            }

            return Ok(ResponseHelper<CourseCreateDto>.Success(null, "Course created successfully"));
        }

        [HttpPut("update-course-by-/{id}")]
        public async Task<IActionResult> UpdateCourse(int id,[FromBody] CourseUpdateDto dto)
        {
            if(id != dto.CourseId)
            {
                return BadRequest(ResponseHelper<CourseUpdateDto>.Fail("Id Mismatch"));
            }

            var success = await _courseRepository.UpdateCourseAsync(dto);
            if (!success)
            {
                return NotFound(ResponseHelper<CourseUpdateDto>.Fail("Course not found or update failed"));
            }

            return Ok(ResponseHelper<CourseUpdateDto>.Success(null, "Course updated"));
        }

        [HttpDelete("delete-course/{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _courseRepository.DeleteCourseAsync(id);
            if (!course)
            {
                return NotFound(ResponseHelper<string>.Fail("Course not found or delete failed"));
            }

            return Ok(ResponseHelper<string>.Success(null,"Course deleted successfully"));
        }

        [HttpPost("assign-teacher")]
        public async Task<IActionResult> AssignTeacherToCourse([FromBody] AssignTeacherDto dto)
        {
            var success = await _courseRepository.AssignTeacherToCourseAsync(dto);

            if (!success)
                return BadRequest(ResponseHelper<AssignTeacherDto>.Fail("Failed to assign teacher to course"));

            return Ok(ResponseHelper<AssignTeacherDto>.Success(null, "Teacher assigned to course successfully"));
        }

    }
}
