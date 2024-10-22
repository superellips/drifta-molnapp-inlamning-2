using TipsRundan.Domain.Entities;

namespace TipsRundan.Application.DataTransferObjects;

/// <summary>
/// Data transfer object to move quizdata to and from the presentation layer.
/// </summary>
public class QuizDTO
{
    public QuizDTO() { }
    public QuizDTO(Quiz quiz)
    {
        Id = quiz.Id;
        Owner = quiz.UserId;
        Title = quiz.Title;
        Questions = quiz.Questions.Select(q => new QuestionDTO(q)).ToList();
         
    }

    public Guid Id { get; set; }
    public Guid Owner { get; set; }
    public string Title { get; set; }
    public List<QuestionDTO> Questions { get; set; }
    public int CurrentQuestion { get; set; }

    internal Quiz MapTopEntity()
    {
        return Quiz.Create(
            Owner,
            Title,
            Questions.Select(q => q.MapToEntity()).ToList()
        );
    }
}