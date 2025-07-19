using Dapper;
using LMS.API.Data;
using LMS.API.Dtos;
using LMS.API.Models;

namespace LMS.API.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        private readonly DapperContext _dapperContext;

        public LessonRepository(DapperContext context)
        {
            _dapperContext = context;
        }


        public async Task<bool> UploadLessonAsync(Lesson lesson)
        {
            var sql = @"INSERT INTO Lessons (CourseId, Title, FilePath, FileType, UploadedBy, UploadedAt) 
                        VALUES (@CourseId, @Title, @FilePath, @FileType, @UploadedBy, @UploadedAt)";
            using var conn = _dapperContext.CreateConnection();
            var result = await  conn.ExecuteAsync(sql, lesson);
            return result > 0;
        }

        public async Task<IEnumerable<LessonResponseDto>> GetLessonsByCourseIdAsync(int courseId)
        {
            var sql = @"SELECT LessonId, CourseId, Title, FilePath, FileType, UploadedAt 
                        FROM Lessons 
                        WHERE CourseId = @CourseId 
                        ORDER BY UploadedAt DESC";

            using var conn = _dapperContext.CreateConnection();
            return   await conn.QueryAsync<LessonResponseDto>(sql, new {CourseId = courseId});
        }
    }
}
