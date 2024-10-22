using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TipsRundan.Domain.Entities;

namespace TipsRundan.Infrastructure.DataModels;

public class AnswerDataModel
{
    [Key]
    public Guid Id { get; set; }
    public int Order { get; set; }
    public string? Text { get; set; }
    public bool IsCorrect { get; set; }
    public bool IsChoosen { get; set; }
    public AnswerDataModel() { }

    public AnswerDataModel(Answer answer)
    {
        Id = answer.Id;
        Text = answer.Text;
        IsCorrect = answer.IsCorrect;
        IsChoosen = answer.IsChoosen;
    }
    public AnswerDataModel(Answer answer, int order) : this(answer)
    {
        Order = order;
    }
    public Answer MapToDomainEntity()
    {
        return new Answer(Text!, IsCorrect);
    }
}