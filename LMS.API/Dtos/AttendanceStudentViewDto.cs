namespace LMS.API.Dtos
{
    public class AttendanceStudentViewDto
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public bool IsPresent { get; set; }
    }

}
