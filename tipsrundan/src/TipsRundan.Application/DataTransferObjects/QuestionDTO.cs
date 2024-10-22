
using TipsRundan.Domain.Entities;

namespace TipsRundan.Application.DataTransferObjects;

/// <summary>
/// Transfer object to move question data to and from the presentation layer.
/// </summary>
public class QuestionDTO
{
    public QuestionDTO() { }
    public QuestionDTO(Question question)
    {
        Id = question.Id;
        Inquiry = question.Inquiry;
        Options = question.Answers.Select(a => a.Text).ToList();
        CorrectAnswer = question.Answers.IndexOf(question.Answers.First(a => a.IsCorrect));
    }
    public Guid Id { get; set; }
    public string? Inquiry { get; set; }
    public List<string>? Options { get; set; }
    public int CorrectAnswer { get; set; }
    public int Answer { get; set; }
    public int NextQuestion { get; set; }

    internal Question MapToEntity()
    {
        var answers = new List<Answer>();
        for (int i = 0; i < Options.Count; i++)
        {
            answers.Add(new Answer(Options[i], i == CorrectAnswer));
        }
        return new Question(
            Inquiry,
            answers
        );
    }
}