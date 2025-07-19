using Dapper;
using LMS.API.Data;
using LMS.API.Dtos;
using LMS.API.Models;
using System.Data;

namespace LMS.API.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly DapperContext _dapperContext;

        public CourseRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        // ✅ Get All Courses
        public async Task<IEnumerable<CourseResponseDto>> GetAllCoursesAsync()
        {
            var sql = @"SELECT c.CourseId, c.Title, c.Description,
                               c.BatchId, b.BatchCode, 
                               c.IsActive, c.CreatedAt
                        FROM Courses c 
                        INNER JOIN Batches b ON c.BatchId = b.BatchId";

            using var conn = _dapperContext.CreateConnection();
            return await conn.QueryAsync<CourseResponseDto>(sql);
        }

        // ✅ Get Course by ID
        public async Task<CourseResponseDto?> GetCourseByIdAsync(int id)
        {
            var sql = @"SELECT c.CourseId, c.Title, c.Description, 
                               c.BatchId, b.BatchCode, 
                               c.IsActive, c.CreatedAt
                        FROM Courses c 
                        INNER JOIN Batches b ON c.BatchId = b.BatchId
                        WHERE c.CourseId = @Id";

            using var conn = _dapperContext.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<CourseResponseDto>(sql, new { Id = id });
        }

        // ✅ Create Course
        public async Task<bool> CreateCourseAsync(CourseCreateDto dto)
        {
            var sql = @"INSERT INTO Courses (Title, Description, BatchId, IsActive, CreatedAt)
                        VALUES (@Title, @Description, @BatchId, 1, @CreatedAt)";

            using var conn = _dapperContext.CreateConnection();
            var result = await conn.ExecuteAsync(sql, new
            {
                dto.Title,
                dto.Description,
                dto.BatchId,
                CreatedAt = DateTime.UtcNow
            });

            return result > 0;
        }

        // ✅ Update Course
        public async Task<bool> UpdateCourseAsync(CourseUpdateDto dto)
        {
            var sql = @"UPDATE Courses 
                        SET Title = @Title, 
                            Description = @Description, 
                            BatchId = @BatchId, 
                            IsActive = @IsActive
                        WHERE CourseId = @CourseId";

            using var conn = _dapperContext.CreateConnection();
            var result = await conn.ExecuteAsync(sql, dto);
            return result > 0;
        }

        // ✅ Delete Course
        public async Task<bool> DeleteCourseAsync(int id)
        {
            var sql = @"DELETE FROM Courses WHERE CourseId = @Id";

            using var conn = _dapperContext.CreateConnection();
            var result = await conn.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }

        // Assign Teacher
        public async Task<bool> AssignTeacherToCourseAsync(AssignTeacherDto dto)
        {
            var sql = @"INSERT INTO CourseTeacherMap (CourseId, TeacherId, AssignedAt)
                VALUES (@CourseId, @TeacherId, NOW())";

            using var conn = _dapperContext.CreateConnection();
            var result = await conn.ExecuteAsync(sql, dto);
            return result > 0;
        }

    }
}
