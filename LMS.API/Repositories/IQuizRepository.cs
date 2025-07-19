using LMS.API.Dtos;

namespace LMS.API.Repositories
{
    public interface IQuizRepository
    {
        Task<bool> PostQuizAsync(QuizCreateDto dto, int teacherId);
        Task<List<QuizViewDto>> GetQuizzesByCourseIdAsync(int courseId);
    }
}
