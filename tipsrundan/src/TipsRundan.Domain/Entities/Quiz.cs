using System.ComponentModel.DataAnnotations;

namespace TipsRundan.Domain.Entities
{
    public class Quiz
    {
        [Key]
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; private set; }

        [MinLength(1, ErrorMessage = "Quiz must have at least one question")]
        public List<Question> Questions { get; private set; }

        // Privat konstruktor
        private Quiz(Guid id, Guid userId, string title, List<Question> questions)
        {
            Id = id;
            UserId = userId;
            Title = title;
            Questions = questions;
        }

        // Factory method för att skapa ett nytt quiz
        public static Quiz Create(Guid userId, string title, List<Question> questions)
        {
            // Genererar ett nytt Id och använder detta vid skapandet
            return new Quiz(Guid.NewGuid(), userId, title, questions);
        }

        // Factory method för att ladda ett quiz från databasen
        public static Quiz Load(Guid quizId, Guid userId, string title, List<Question> questions)
        {
            // Använder det befintliga Id och laddar quizet
            return new Quiz(quizId, userId, title, questions);
        }
    }
}
