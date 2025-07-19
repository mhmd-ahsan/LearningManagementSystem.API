namespace LMS.API.Dtos
{
    public class CourseUpdateDto
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int BatchId { get; set; }
        public bool IsActive { get; set; }
    }

}
