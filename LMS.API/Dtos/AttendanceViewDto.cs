namespace LMS.API.Dtos
{
    public class AttendanceViewDto
    {
        public int CourseId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public List<AttendanceStudentViewDto> Students { get; set; }
    }

}
