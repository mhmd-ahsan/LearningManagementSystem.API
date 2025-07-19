namespace LMS.API.Dtos
{
    public class QuizViewDto
    {
        public int QuizId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public List<QuizQuestionViewDto> Questions { get; set; } = new();
    }
}
