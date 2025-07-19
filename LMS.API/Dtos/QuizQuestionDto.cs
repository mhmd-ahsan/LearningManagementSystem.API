﻿namespace LMS.API.Dtos
{
    public class QuizQuestionDto
    {
        public  string QuestionText { get; set; } = string.Empty;
        public  string OptionA { get; set; } = string.Empty;
        public  string OptionB { get; set; } = string.Empty;
        public  string OptionC { get; set; } = string.Empty;
        public  string OptionD { get; set; } = string.Empty;
        public  string CorrectOption { get; set; } = string.Empty;
    }
}
