namespace LMS.API.Models
{
    public class Batch
    {
        public int BatchId { get; set; }                   // Primary Key
        public string BatchCode { get; set; } = string.Empty; // Unique code shared with students
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Timing { get; set; } = string.Empty;    // e.g., "9:00 AM - 11:00 AM"
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
