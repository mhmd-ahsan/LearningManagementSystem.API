namespace LMS.API.Dtos
{
    public class LessonCreateDto
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public IFormFile File { get; set; } = null!;
    }
}
