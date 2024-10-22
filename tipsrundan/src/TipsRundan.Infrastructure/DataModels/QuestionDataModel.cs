using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TipsRundan.Domain.Entities;

namespace TipsRundan.Infrastructure.DataModels;

public class QuestionDataModel
{
    [Key]
    public Guid Id { get; set; }
    public int Order { get; set; }
    public string? Inquiry { get; set; }
    public List<AnswerDataModel>? Answers { get; set; }
    public QuestionDataModel() { }
    public QuestionDataModel(Question question) 
    {
        Id = question.Id;
        Inquiry = question.Inquiry;
        Answers = [];
        var ordering = 0;
        foreach (var answer in question.Answers)
        {
            Answers.Add(new AnswerDataModel(answer, ordering));
            ordering++;
        }
    }
    public QuestionDataModel(Question question, int order) : this(question)
    {
        Order = order;
    }
    public Question MapToDomainEntity()
    {
        var answerEntities = (List<Answer>)[];
        foreach (var a in Answers!.OrderBy(a => a.Order))
        {
            answerEntities.Add(a.MapToDomainEntity());
        }
        return new Question(Inquiry!, answerEntities);
    }
}