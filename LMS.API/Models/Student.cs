namespace LMS.API.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public int BatchId { get; set; }
        public int UserId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
