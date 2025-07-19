namespace LMS.API.Dtos
{
    public class QuizAttemptDto
    {
        public int QuizId { get; set; }
        public List<QuizAnswerDto> Answers { get; set; } = new();
    
    }
}
