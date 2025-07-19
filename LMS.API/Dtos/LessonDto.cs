namespace LMS.API.Dtos
{
    public class LessonDto
    {
        public int LessonId { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
