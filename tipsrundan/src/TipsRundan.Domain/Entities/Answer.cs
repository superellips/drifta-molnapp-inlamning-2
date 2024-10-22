using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TipsRundan.Domain.Entities
{
    public class Answer
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Answer text is too long")]
        public string Text { get; private set; }

        [Required]
        [ForeignKey("Question")]
        public Guid QuestionId { get; set; }

        public bool IsCorrect { get; private set; }

        public bool IsChoosen { get; private set; }

        public Answer(string text, bool isCorrect)
        {
            Text = text;
            IsCorrect = isCorrect;
        }

        public void MarkAsChoosen()
        {
            IsChoosen = true;
        }
    }
}