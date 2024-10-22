using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TipsRundan.Domain.Entities
{
    public class Question
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Inquiry is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Inquiry must be between 1 and 100 characters long.")]
        public string Inquiry { get; private set; }

        [ForeignKey("Quiz")]
        public Guid QuizId { get; set; }

        [Required(ErrorMessage = "Answers are required.")]
        [MinLength(2, ErrorMessage = "Question must have at least 2 answers.")]
        [MaxLength(10, ErrorMessage = "Question can have at most 10 answers.")]
        [CustomValidation(typeof(Question), nameof(ValidateAnswers))]
        public List<Answer> Answers { get; private set; }

        public Question(string inquiry, List<Answer> answers)
        {
            Inquiry = inquiry;
            Answers = answers;
        }

        public static ValidationResult ValidateAnswers(List<Answer> answers, ValidationContext context)
        {
            if (answers.Count(a => a.IsCorrect) < 1)
            {
                return new ValidationResult("Questions must have at least one correct answer.");
            }

            return ValidationResult.Success;
        }
    }
}