using TipsRundan.Application.DataTransferObjects;

namespace TipsRundan.Web.Models;

public class TakeQuizView
{
    public TakeQuizView() { }
    public TakeQuizView(QuizDTO quiz)
    {
        Id = quiz.Id;
        Answers = new List<int>();
        Questions = new List<QuestionView>();
        foreach (var question in quiz.Questions)
        {
            Answers.Add(0);
            Questions.Add(new QuestionView(question));
        }
    }
    public Guid Id { get; set; }
    public List<int>? Answers { get; set; }
    public List<QuestionView?>? Questions { get; set; }
}