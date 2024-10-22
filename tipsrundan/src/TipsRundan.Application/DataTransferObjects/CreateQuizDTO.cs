namespace TipsRundan.Application.DataTransferObjects
{
    public class CreateQuizDTO // Data Transfer Object. Samlar in nödvändig data för att skapa ett quiz
    {
        public string Title { get; set; } // Titel för quizet
        public string Description { get; set; } // Beskrivning av quizet
        public List<QuestionDTO> Questions { get; set; } = new List<QuestionDTO>(); // Lista över frågor som tillhör quizet
    }
}