using LMS.API.Dtos;
using LMS.API.Models;

namespace LMS.API.Repositories
{
    public interface IStudentRepository
    {
        Task<bool> EnrollStudentInCourseAsync(StudentCourseEnrollDto dto);
        Task<IEnumerable<CourseDto>> GetEnrolledCoursesAsync(int studentId);
        Task<Student?> GetByUserIdAsync(int userId);


        // 🔹 NEW: Fetch lessons for enrolled courses
        Task<IEnumerable<LessonDto>> GetLessonsForStudentAsync(int studentId);
        Task<QuizResultDto> SubmitQuizAsync(QuizAttemptDto attemptDto, int studentId);
        Task<IEnumerable<QuizQuestionDto>> GetQuizQuestionsAsync(int quizId);
        Task<IEnumerable<AttendanceDto>> GetAttendanceByStudentIdAsync(int studentId);



    }
}
