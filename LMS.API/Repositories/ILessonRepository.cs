using LMS.API.Dtos;
using LMS.API.Models;

namespace LMS.API.Repositories
{
    public interface ILessonRepository
    {
        Task<bool> UploadLessonAsync(Lesson lesson);
        Task<IEnumerable<LessonResponseDto>> GetLessonsByCourseIdAsync(int courseId);
    }
}
