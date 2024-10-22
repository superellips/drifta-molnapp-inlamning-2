using TipsRundan.Application.DataTransferObjects;
using TipsRundan.Application.Interfaces;
using TipsRundan.Domain.Interfaces;


namespace TipsRundan.Application.Services;

public class NewQuizService(IQuizRepository repository, IAuthnService authnService) : IQuizService
{
    private readonly IQuizRepository _repository = repository;
    private readonly IAuthnService _accountService = authnService;

    public void AddQuiz(QuizDTO quiz)
    {
        var user = _accountService.GetCurrentUser();
        quiz.Owner = user.Id;

        _repository.Create(quiz.MapTopEntity());
    }

    public QuizDTO GetQuizById(Guid id)
    {
        return new QuizDTO(_repository.ReadById(id));
    }

    public List<QuizDTO> GetQuizes()
    {
        return _repository.ReadAll().Select(q => new QuizDTO(q)).ToList();
    }

    public QuizDTO UpdateQuiz(Guid quizId, QuestionDTO question, int number)
    {
        var quiz = _repository.ReadById(quizId);
        quiz.Questions[number]
                .Answers[question.Answer]
                .MarkAsChoosen();
        return new QuizDTO(_repository.Update(quiz))
        {
            CurrentQuestion = question.NextQuestion
        };
    }
}