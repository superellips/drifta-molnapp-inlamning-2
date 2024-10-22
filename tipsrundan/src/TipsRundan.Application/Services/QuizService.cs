using TipsRundan.Application.DataTransferObjects;

namespace TipsRundan.Application.Services;


/// <summary>
/// A simple in memory implementation of IQuizService.
/// The primary purpose was to move some of logic away from the 
/// controller in the presentation layer.
/// </summary>
public class QuizService : IQuizService
{
        private List<QuizDTO> _quizes;

        public QuizService()
        {
                var quiz = new QuizDTO(){
                        Id = Guid.NewGuid(),
                        Title = "Our First Quiz",
                        Questions = [
                                new(){
                                        Inquiry = "If \"Model\" in MVC were a type of food, what would it be?",
                                        CorrectAnswer = 0,
                                        Options = [ 
                                                "A hamburger, because it holds everything together and is a bit meaty", 
                                                "A salad, because it stays fresh and organized", 
                                                "A lasagna, because it has layers of data neatly stacked together" 
                                        ]
                                },
                                new(){
                                        Inquiry = "If MVC were a rock band, what role would the \"Controller\" play?",
                                        CorrectAnswer = 0,
                                        Options = [ 
                                                "The drummer, keeping the beat and deciding when things should happen", 
                                                "The lead singer, always in the spotlight and getting all the attention", 
                                                "The guitarist, just playing cool riffs without caring about what else is going on" 
                                        ]
                                },
                                new(){
                                        Inquiry = "If \"View\" were a person at a party, what would they be doing?",
                                        CorrectAnswer = 2,
                                        Options = [ 
                                                "Standing by the door greeting everyone, because they love being the first impression", 
                                                "Hiding in the kitchen, just observing what’s going on", 
                                                "Standing on the table and putting on a big show, because they love to be seen and heard"
                                        ]
                                },
                                new(){
                                        Inquiry = "If the Controller had a superpower, what would it be?",
                                        CorrectAnswer = 1,
                                        Options = [ 
                                                "Super strength, because it has to handle all the user’s demands", 
                                                "Telepathy, because it needs to understand what both Model and View are thinking", 
                                                "Time travel, to make sure everything happens at the right moment"
                                        ]
                                },
                                new(){
                                        Inquiry = "If you were an MVC app, which part would you ask for advice when you don’t know what to do?",
                                        CorrectAnswer = 2,
                                        Options = [ 
                                                "Model, because it has all the facts and logic", 
                                                "View, because it has strong opinions on how things should look", 
                                                "Controller, because it’s used to handling everything and making it work" 
                                        ]
                                }
                        ]
                };
                _quizes = [quiz];
        }

        public List<QuizDTO> GetQuizes()
        {
                return _quizes;
        }

        public QuizDTO GetQuizById(Guid id)
        {
                return _quizes.FirstOrDefault(q => q.Id == id);
        }

        public QuizDTO UpdateQuiz(Guid quizId, QuestionDTO question, int number)
        {
                var quiz = _quizes.FirstOrDefault(q => q.Id == quizId);
                quiz.Questions[quiz.CurrentQuestion].Answer = question.Answer;
                quiz.CurrentQuestion = question.NextQuestion < quiz.Questions.Count ? question.NextQuestion : quiz.CurrentQuestion;
                return quiz;
        }

    public void AddQuiz(QuizDTO newQuiz)
    {
        if (newQuiz.Id == Guid.Empty)
        {
            newQuiz.Id = Guid.NewGuid(); // Assign a new unique ID if not provided
        }
        _quizes.Add(newQuiz);
    }

    public void AddQuiz(CreateQuizDTO newQuizDTO)
    {
        throw new NotImplementedException();
    }
}
