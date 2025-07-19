using LMS.API.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS.API.Repositories
{
    public interface ICourseRepository
    {
        Task<IEnumerable<CourseResponseDto>> GetAllCoursesAsync();
        Task<CourseResponseDto?> GetCourseByIdAsync(int id);
        Task<bool> CreateCourseAsync(CourseCreateDto course);
        Task<bool> UpdateCourseAsync(CourseUpdateDto course);
        Task<bool> DeleteCourseAsync(int id);
        Task<bool> AssignTeacherToCourseAsync(AssignTeacherDto dto);

    }
}
