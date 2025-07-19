namespace LMS.API.Dtos
{
    public class AttendanceCreateDto
    {
        public int CourseId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public List<StudentAttendanceDto> Students { get; set; }
    }
}
