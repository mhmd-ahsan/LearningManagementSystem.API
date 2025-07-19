namespace LMS.API.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public int UserId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
