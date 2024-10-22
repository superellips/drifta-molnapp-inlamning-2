using System.ComponentModel.DataAnnotations;
using TipsRundan.Domain.Entities;

namespace TipsRundan.Infrastructure.DataModels;

public class QuizDataModel
{
    [Key]
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? Title { get; set; }
    public List<QuestionDataModel>? Questions { get; set; }
    public QuizDataModel() { }
    public QuizDataModel(Quiz quiz)
    {
        Id = quiz.Id;
        UserId = quiz.UserId;
        Title = quiz.Title;
        Questions = [];
        var ordering = 0;
        foreach (var question in quiz.Questions)
        {
            Questions.Add(new QuestionDataModel(question, ordering));
            ordering++;
        }
    }
    public Quiz MapToDomainEntity()
    {
        var questionEntities = (List<Question>)[];
        foreach (var q in Questions!.OrderBy(q => q.Order))
        {
            questionEntities.Add(q.MapToDomainEntity());
        }
        return Quiz.Load(Id, UserId, Title!, questionEntities);
    }
}