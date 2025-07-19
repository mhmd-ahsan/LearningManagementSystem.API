namespace LMS.API.Dtos
{
    public class CourseResponseDto
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int BatchId { get; set; }
        public string BatchCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
