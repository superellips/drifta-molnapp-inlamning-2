using TipsRundan.Application.DataTransferObjects;

namespace TipsRundan.Application.Services;

/// <summary>
/// A small example of how we could write an interface to move
/// some of the logic further away from the presentation layer.
/// </summary>
public interface IQuizService
{
        public List<QuizDTO> GetQuizes();
        public QuizDTO GetQuizById(Guid id);
        public QuizDTO UpdateQuiz(Guid quizId, QuestionDTO question, int number);
        public void AddQuiz(QuizDTO quiz);
}