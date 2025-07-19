using LMS.API.Dtos;

namespace LMS.API.Repositories
{
    public interface IAttendanceRepository
    {
        Task<bool> MarkAttendanceAsync(AttendanceCreateDto dto, int teacherId);
        Task<AttendanceViewDto?> GetAttendanceByCourseAndDateAsync(int courseId, DateTime date);

    }
}
