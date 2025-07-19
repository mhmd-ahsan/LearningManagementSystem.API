namespace LMS.API.Dtos
{
    public class QuizCreateDto
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<QuizQuestionDto> Questions { get; set; } = new();
    }
}
