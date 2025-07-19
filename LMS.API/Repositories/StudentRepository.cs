using Dapper;
using LMS.API.Data;
using LMS.API.Dtos;
using LMS.API.Models;
using MySql.Data.MySqlClient;

namespace LMS.API.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly DapperContext _dapperContext;

        public StudentRepository(DapperContext context)
        {
            _dapperContext = context;
        }

        public async Task<bool> EnrollStudentInCourseAsync(StudentCourseEnrollDto enrollDto)
        {
            var sql = @"INSERT INTO studentcourses (StudentId, CourseId, EnrolledAt)
                        VALUES (@StudentId, @CourseId, NOW());";
            using var conn = (MySqlConnection) _dapperContext.CreateConnection();
            await conn.OpenAsync();
            using var transaction = await conn.BeginTransactionAsync();

            try
            {
                await conn.ExecuteAsync(sql, new
                {
                    enrollDto.StudentId,
                    enrollDto.CourseId,
                }, transaction);

                await transaction.CommitAsync();
                return true;
            }

            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<IEnumerable<CourseDto>> GetEnrolledCoursesAsync(int studentId)
        {
            var query = @"
        SELECT c.CourseId, c.Title, c.Description, c.IsActive, c.CreatedAt
        FROM Course_Info c
        INNER JOIN studentcourses sc ON c.CourseId = sc.CourseId
        WHERE sc.StudentId = @StudentId;
    ";

            using var connection = _dapperContext.CreateConnection();
            var courses = await connection.QueryAsync<CourseDto>(query, new { StudentId = studentId });
            return courses;
        }

        public async Task<Student?> GetByUserIdAsync(int userId)
        {
            var sql = @"SELECT FROM Students WHERE UserId = @UserId";
            using var conn = _dapperContext.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<Student>(sql, new { UserId = userId });
        }
        public async Task<IEnumerable<LessonDto>> GetLessonsForStudentAsync(int studentId)
        {
            var query = @"
        SELECT l.LessonId, l.Title, l.FilePath, l.FileType, l.UploadedAt
        FROM lessons l
        INNER JOIN Course_Info c ON l.CourseId = c.CourseId
        INNER JOIN studentcourses sc ON c.CourseId = sc.CourseId
        WHERE sc.StudentId = @StudentId;
    ";

            using var connection = _dapperContext.CreateConnection();
            var lessons = await connection.QueryAsync<LessonDto>(query, new { StudentId = studentId });
            return lessons;
        }
        public async Task<QuizResultDto> SubmitQuizAsync(QuizAttemptDto attemptDto, int studentId)
        {
            var query = @"
        SELECT QuestionId, CorrectOption
        FROM quizquestions
        WHERE QuizId = @QuizId;
    ";

            using var connection = _dapperContext.CreateConnection();

            // Fetch all correct answers from DB
            var correctAnswers = await connection.QueryAsync<QuizAnswerDto>(query, new { QuizId = attemptDto.QuizId });

            int total = correctAnswers.Count();
            int correct = 0;

            foreach (var submittedAnswer in attemptDto.Answers)
            {
                var correctAnswer = correctAnswers.FirstOrDefault(q => q.QuestionId == submittedAnswer.QuestionId);
                if (correctAnswer != null && submittedAnswer.SelectedOption.ToUpper() == correctAnswer.SelectedOption.ToUpper())
                {
                    correct++;
                }
            }

            return new QuizResultDto
            {
                TotalQuestions = total,
                CorrectAnswers = correct
            };
        }
        public async Task<IEnumerable<QuizQuestionDto>> GetQuizQuestionsAsync(int quizId)
        {
            var query = @"
        SELECT QuestionId, QuestionText, OptionA, OptionB, OptionC, OptionD
        FROM quizquestions
        WHERE QuizId = @QuizId;
    ";

            using var connection = _dapperContext.CreateConnection();
            var questions = await connection.QueryAsync<QuizQuestionDto>(query, new { QuizId = quizId });
            return questions;
        }

        public async Task<IEnumerable<AttendanceDto>> GetAttendanceByStudentIdAsync(int studentId)
        {
            var query = @"
        SELECT 
            AttendanceId, 
            CourseId, 
            AttendanceDate, 
            IsPresent
        FROM attendances
        WHERE StudentId = @StudentId
        ORDER BY AttendanceDate DESC;
    ";

            using var connection = _dapperContext.CreateConnection();
            var attendanceRecords = await connection.QueryAsync<AttendanceDto>(query, new { StudentId = studentId });
            return attendanceRecords;
        }

    }
}
