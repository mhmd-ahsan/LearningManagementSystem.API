namespace LMS.API.Models
{
    public class Attendance
    {
        public int AttendanceId { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public bool IsPresent { get; set; }
        public int MarkedBy { get; set; }
        public DateTime MarkedAt { get; set; }
    }
}
