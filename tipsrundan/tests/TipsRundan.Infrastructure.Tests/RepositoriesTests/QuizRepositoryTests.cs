using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TipsRundan.Domain.Entities;
using TipsRundan.Domain.Interfaces;
using TipsRundan.Infrastructure.DbContexts;
using TipsRundan.Infrastructure.Repositories;

namespace TipsRundan.Infrastructure.Tests.RepositoriesTests;

public class QuizRepositoryTests : IDisposable
{
    private readonly List<Quiz> _quizes = 
        [ 
            Quiz.Create(Guid.NewGuid(),"Our Test Quiz", [
            new Question(
                "Am I the first question?", 
                [ 
                    new Answer("yes", false), 
                    new Answer("no", true) 
                ]
            )
            ]),Quiz.Create(Guid.NewGuid(),"Our Test Quiz 2", [
                new Question(
                    "Am I the first question?", 
                    [ 
                        new Answer("yes", false), 
                        new Answer("no", true) 
                    ]
                )
            ]),Quiz.Create(Guid.NewGuid(),"Our Test Quiz 3", [
                new Question(
                    "Am I the first question?", 
                    [ 
                        new Answer("yes", false), 
                        new Answer("no", true) 
                    ]
                )
            ]),Quiz.Create(Guid.NewGuid(),"Our Test Quiz 4", [
                new Question(
                    "Am I the first question?", 
                    [ 
                        new Answer("yes", false), 
                        new Answer("no", true) 
                    ]
                )
            ])
        ];
    private readonly SqliteConnection _connection;
    private readonly SqliteDbContext _context;
    private readonly IQuizRepository _subject;

    public QuizRepositoryTests()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();
        var options = new DbContextOptionsBuilder<SqliteDbContext>().UseSqlite(_connection).Options;
        _context = new SqliteDbContext(options);
        _subject = new QuizRepository(_context);
        SeedDatabase();
    }
    public void Dispose()
    {
        // Dispose the context and connection
        _context.Dispose();
        _connection.Close();
        _connection.Dispose();
    }

    [Fact]
    public void Create_ShouldReturnTheQuizEntity()
    {
        // Arrange
        var expectedQuestions = (List<Question>)[
            new Question("Am I the first question?", [ new Answer("yes", false), new Answer("no", true) ])
        ];
        var userId = Guid.NewGuid();
        var expectedQuiz = Quiz.Create(userId,"Another Test Quiz", expectedQuestions);

        // Act
        var actual = _subject.Create(expectedQuiz);

        // Assert
        Assert.Equal(expectedQuiz.Title, actual.Title);
    }

    [Fact]
    public void Create_ShouldReturnANewQuizWithTheCreatedGuid()
    {
        // Arrange
        var expectedQuestions = (List<Question>)[
            new Question("Am I the first question?", [ new Answer("yes", false), new Answer("no", true) ])
        ];
        var userId = Guid.NewGuid();
        var expectedQuiz = Quiz.Create(userId,"Another Test Quiz", expectedQuestions);

        // Act
        var actual = _subject.Create(expectedQuiz);

        // Assert
        Assert.NotEqual(Guid.Empty, actual.Id);
    }

    [Fact]
    public void ReadById_ShouldReturnTheExpectedQuiz()
    {
        // Arrange
        var expectedQuestions = (List<Question>)[
            new Question("Am I the first question?", [ new Answer("yes", false), new Answer("no", true) ])
        ];
        var expectedUserId = Guid.NewGuid();
        var expectedQuiz = Quiz.Create(expectedUserId,"Our Test Quiz", expectedQuestions);

        var unexpectedQuestions = (List<Question>)[
            new Question("I am not a question", [ new Answer("What?", false), new Answer("Why?", true) ])
        ];
        var unexpectedUserId = Guid.NewGuid();
        var unexpectedQuiz = Quiz.Create(unexpectedUserId,"Not Our Test Quiz", unexpectedQuestions);

        expectedQuiz = _subject.Create(expectedQuiz);
        unexpectedQuiz = _subject.Create(unexpectedQuiz);

        // Act
        var actual = _subject.ReadById(expectedQuiz.Id);

        // Assert
        Assert.Equal(expectedQuiz.Id, actual.Id);
        Assert.NotEqual(unexpectedQuiz.Id, actual.Id);
        Assert.Equal(expectedQuiz.Title, actual.Title);
    }

    [Fact]
    public void ReadById_ShouldReturnNullWhenQuizNotPresent()
    {
        // Arrange

        // Act
        var actual = _subject.ReadById(Guid.Empty);

        // Assert
        Assert.True(actual is null);
    }

    [Fact]
    public void ReadAll_ShouldReturnAListOfQuizes()
    {
        // Arrange

        // Act
        var actual = _subject.ReadAll();

        // Assert
        Assert.True(actual is not null);
        Assert.True(actual.Count == _quizes.Count);
    }

    private void SeedDatabase()
    {
        
        foreach (var quiz in _quizes)
        {
            _subject.Create(quiz);
        }
    }
}
