using Dapper;
using LMS.API.Data;
using LMS.API.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using MySql.Data.MySqlClient;
using System.Linq.Expressions;

namespace LMS.API.Repositories
{
    public class QuizRepository : IQuizRepository
    {
        private readonly DapperContext _dapperContext;

        public QuizRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }


        public async Task<bool> PostQuizAsync(QuizCreateDto dto, int teacherId)
        {
            var quizSql = @"INSERT INTO quizzes 
                (CourseId, Title, Description, CreatedBy, CreatedAt) 
                VALUES (@CourseId, @Title, @Description, @CreatedBy, @CreatedAt);
                SELECT LAST_INSERT_ID();";

            var questionSql = @"INSERT INTO quizquestions 
                (QuizId, QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectOption) 
                VALUES (@QuizId, @QuestionText, @OptionA, @OptionB, @OptionC, @OptionD, @CorrectOption);";

            using var conn = (MySqlConnection)_dapperContext.CreateConnection();
            await conn.OpenAsync();
            using var transaction = await conn.BeginTransactionAsync();

            try
            {
                var quizId = await conn.ExecuteScalarAsync<int>(quizSql, new
                {
                    dto.CourseId,
                    dto.Title,
                    dto.Description,
                    CreatedBy = teacherId,
                    CreatedAt = DateTime.Now
                }, transaction);

                foreach (var question in dto.Questions)
                {
                    await conn.ExecuteAsync(questionSql, new
                    {
                        QuizId = quizId,
                        question.QuestionText,
                        question.OptionA,
                        question.OptionB,
                        question.OptionC,
                        question.OptionD,
                        question.CorrectOption
                    }, transaction);
                }

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<List<QuizViewDto>> GetQuizzesByCourseIdAsync(int courseId)
        {
            var sqlQuizzes = @"SELECT QuizId, Title, Description, CreatedAt 
                       FROM quizzes 
                       WHERE CourseId = @CourseId";

            var sqlQuestions = @"SELECT QuestionId, QuizId, QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectOption 
                         FROM quizquestions 
                         WHERE QuizId = @QuizId";

            using var conn = (MySqlConnection)_dapperContext.CreateConnection();
            await conn.OpenAsync();

            var quizzes = (await conn.QueryAsync<QuizViewDto>(sqlQuizzes, new { CourseId = courseId })).ToList();

            foreach (var quiz in quizzes)
            {
                var questions = (await conn.QueryAsync<QuizQuestionViewDto>(sqlQuestions, new { QuizId = quiz.QuizId })).ToList();
                quiz.Questions = questions;
            }

            return quizzes;
        }

    }
}