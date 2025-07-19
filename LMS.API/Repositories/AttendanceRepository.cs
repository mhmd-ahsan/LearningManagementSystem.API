using Dapper;
using LMS.API.Data;
using LMS.API.Dtos;
using LMS.API.Helpers;
using Microsoft.AspNetCore.Http.HttpResults;
using MySql.Data.MySqlClient;

namespace LMS.API.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly DapperContext _context;

        public AttendanceRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<bool> MarkAttendanceAsync(AttendanceCreateDto dto, int teacherId)
{
    var insertSql = @"INSERT INTO attendances 
                    (CourseId, StudentId, AttendanceDate, IsPresent, MarkedBy, MarkedAt)
                    VALUES (@CourseId, @StudentId, @AttendanceDate, @IsPresent, @MarkedBy, @MarkedAt)";
                    
    var checkSql = @"SELECT COUNT(*) FROM attendances 
                     WHERE CourseId = @CourseId AND StudentId = @StudentId AND AttendanceDate = @AttendanceDate";

    using var connection = (MySqlConnection)_context.CreateConnection();
    await connection.OpenAsync();
    using var transaction = await connection.BeginTransactionAsync();

    try
    {
        foreach (var student in dto.Students)
        {
            var exists = await connection.ExecuteScalarAsync<int>(checkSql, new
            {
                CourseId = dto.CourseId,
                StudentId = student.StudentId,
                AttendanceDate = dto.AttendanceDate.Date
            }, transaction);

            if (exists > 0)
                continue; // Already marked for this student/date/course

            await connection.ExecuteAsync(insertSql, new
            {
                CourseId = dto.CourseId,
                StudentId = student.StudentId,
                AttendanceDate = dto.AttendanceDate.Date,
                IsPresent = student.IsPresent,
                MarkedBy = teacherId,
                MarkedAt = DateTime.Now
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


        public async Task<AttendanceViewDto?> GetAttendanceByCourseAndDateAsync(int courseId, DateTime date)
        {
            var sql = @"
        SELECT a.StudentId, s.FullName, a.IsPresent
        FROM attendances a
        INNER JOIN students s ON a.StudentId = s.StudentId
        WHERE a.CourseId = @CourseId AND a.AttendanceDate = @Date";

            try
            {
                using var conn = (MySqlConnection)_context.CreateConnection();
                await conn.OpenAsync();

                var students = (await conn.QueryAsync<AttendanceStudentViewDto>(sql, new
                {
                    CourseId = courseId,
                    Date = date.Date
                })).ToList();

                if (students.Count == 0)
                    return null;

                return new AttendanceViewDto
                {
                    CourseId = courseId,
                    AttendanceDate = date.Date,
                    Students = students
                };
            }
            catch (Exception)
            {
                // You could log the exception here if needed
                return null; // Let controller handle it
            }
        }


    }
}
