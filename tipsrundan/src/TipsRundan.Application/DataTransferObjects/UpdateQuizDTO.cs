namespace TipsRundan.Application.DataTransferObjects
{
    public class UpdateQuizDTO
    {
        public Guid Id { get; set; } // Identifierar det befintliga quizet
        public string Title { get; set; } // Titel för quizet
        public string Description { get; set; } // Beskrivning av quizet
        public List<UpdateQuestionDTO> Questions { get; set; } = new List<UpdateQuestionDTO>(); // Lista över frågor som tillhör quizet
    }

    public class UpdateQuestionDTO : QuestionDTO // Ärver från QuestionDTO
    {
        public Guid Id { get; set; } // Hittar frågan som ska uppdateras.
    }
}