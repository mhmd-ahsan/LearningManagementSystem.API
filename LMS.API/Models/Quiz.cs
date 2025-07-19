namespace LMS.API.Models
{
    public class Quiz
    {
        public int QuizId { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
