namespace LMS.API.Models
{
    public class Lesson
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty ;
        public string FileType { get; set; } = string.Empty;
        public int UploadedBy { get; set; }
        public DateTime UploadedAt { get; set; }

    }
}
