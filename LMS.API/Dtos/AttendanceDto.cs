namespace LMS.API.Dtos
{
    public class AttendanceDto
    {
        public int AttendanceId { get; set; }
        public int CourseId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public bool IsPresent { get; set; }
    }
}
