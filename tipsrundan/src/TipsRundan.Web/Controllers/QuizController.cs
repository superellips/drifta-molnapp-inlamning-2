using Microsoft.AspNetCore.Mvc;
using TipsRundan.Web.Models;
using TipsRundan.Application.Services;
using TipsRundan.Application.DataTransferObjects;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Authorization;

namespace TipsRundan.Controllers;

public class QuizController : Controller
{
    private readonly IQuizService _quizService;

    public QuizController(IQuizService quizService)
    {
        _quizService = quizService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var quizzes = _quizService.GetQuizes();
        return View(quizzes);
    }

    public IActionResult Details(Guid id)
    {
        // Hämtar quizzet efter id.
        var quiz = _quizService.GetQuizById(id);
        if (quiz == null)
        {
            return NotFound();
        }
        return View(quiz);
    }

    [Authorize]
    [HttpGet]
    public IActionResult Create()
    {
        var model = new CreateQuizView
        {
            Questions = new List<QuestionView>
            {
                new QuestionView { Inquiry = "", Options = new List<string> { "", "", "" } }
            },
            CorrectAnswers = [0]
        };

        return View(model);
    }

    [Authorize]
    [HttpPost]
    public IActionResult Create(CreateQuizView quizView)
    {
        // Lägg till en ny tom fråga om användaren klickade på "Add Question"
        if (Request.Form["AddQuestion"] == "true")
        {
            quizView.Questions.Add(new QuestionView { Inquiry = "", Options = new List<string> { "", "", "" } });
            quizView.CorrectAnswers.Add(0);
            return View(quizView); // Visa vyn igen med den nya frågan
        }

        if (ModelState.IsValid)
        {
            var newQuizDTO = new CreateQuizDTO
            {
            Title = quizView.Title,
            Description = quizView.Description,
            Questions = quizView.Questions.Select(question => new QuestionDTO
              {
                Inquiry = question.Inquiry,
                Options = question.Options.ToList(),
                CorrectAnswer = quizView.CorrectAnswers[quizView.Questions.IndexOf(question)]
              }).ToList()
            };

            // Korrigering här: Det ska vara void-metod
            var quizDTO = new QuizDTO
            {
                Title = newQuizDTO.Title,
                Questions = newQuizDTO.Questions.Select(q => new QuestionDTO
                {
                    Inquiry = q.Inquiry,
                    Options = q.Options.ToList(),
                    CorrectAnswer = q.CorrectAnswer
                }).ToList()
            };
            
            _quizService.AddQuiz(quizDTO);

            // Hämta det skapade quizzet igen om du vill visa detaljer
            var createdQuiz = _quizService.GetQuizes()
                                          .FirstOrDefault(q => q.Title == newQuizDTO.Title) ?? throw new Exception("Quiz not created properly.");
            return RedirectToAction("Details", new { id = createdQuiz.Id }); // Om du har en Index-vy för att visa alla quiz
        }

        return View(quizView);
    }

    public IActionResult List()
    {
        var quizes = _quizService.GetQuizes();
        var quizList = new ListView
        {
            QuizIds = quizes.Select(q => q.Id).ToList(),
            QuizTitles = quizes.Select(q => q.Title).ToList()
        };
        return View(quizList);
    }

    public IActionResult TakeQuiz()
    {
        return RedirectToAction("List");
    }

    [Route("Quiz/TakeQuiz/{id:Guid}")]
    public IActionResult TakeQuiz(Guid id)
    {
        var quiz = _quizService.GetQuizById(id);
        return View(new TakeQuizView(quiz));
    }

    [HttpPost]
    [Route("Quiz/Result")]
    public IActionResult Result(TakeQuizView model)
    {
        var quiz = _quizService.GetQuizById(model.Id);
        var result = new ResultView(){
            Questions = quiz.Questions.Select(q => q.Inquiry).ToList(),
            Options = quiz.Questions.Select(q => q.Options.ToArray()).ToList(),
            ChoosenOptions = model.Answers,
            CorrectOptions = quiz.Questions.Select(q => q.CorrectAnswer).ToList(),
            NumberOfQuestions = model.Answers.Count
        };
        for (int i = 0; i < result.CorrectOptions.Count; i++)
        {
            if (result.CorrectOptions[i] == result.ChoosenOptions[i])
            {
                result.NumberOfCorrectAnswers++;
            }
        }
        return View(result);
    }
}
