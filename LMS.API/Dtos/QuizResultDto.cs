namespace LMS.API.Dtos
{
    public class QuizResultDto
    {
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public int IncorrectAnswers => TotalQuestions - CorrectAnswers;
    }
}
