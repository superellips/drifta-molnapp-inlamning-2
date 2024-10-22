using Microsoft.EntityFrameworkCore;
using TipsRundan.Domain.Entities;
using TipsRundan.Domain.Interfaces;
using TipsRundan.Infrastructure.DataModels;
using TipsRundan.Infrastructure.DbContexts;

namespace TipsRundan.Infrastructure.Repositories;

public class QuizRepository : IQuizRepository
{
    private readonly SqliteDbContext _dbContext;
    public QuizRepository(SqliteDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public Quiz Create(Quiz quiz)
    {
        var dataModelQuiz = new QuizDataModel(quiz);
        dataModelQuiz.Id = _dbContext.Add(dataModelQuiz).CurrentValues.GetValue<Guid>(nameof(QuizDataModel.Id));
        _dbContext.SaveChanges();
        return dataModelQuiz.MapToDomainEntity();
    }

    public Quiz Delete(Quiz quiz)
    {
        QuizDataModel deleteQuizDataModel = new QuizDataModel(quiz);
        if (_dbContext.Set<QuizDataModel>().Any(q => q.Id == quiz.Id))
        {
            _dbContext.Remove(deleteQuizDataModel);
            _dbContext.SaveChanges();
            return deleteQuizDataModel.MapToDomainEntity();
        }
        return null;
        
    }

    public List<Quiz> ReadAll()
    {
        var dataModelQuizzes = _dbContext.Quizes
            .Include(quiz => quiz.Questions)
                .ThenInclude(question => question.Answers)
            .ToList();
        return dataModelQuizzes.Select(q => q.MapToDomainEntity()).ToList();
    }

    public Quiz ReadById(Guid id)
    {
        var dataModelQuiz = ReadDataModelById(id);
        return dataModelQuiz?.MapToDomainEntity();

    }

    public Quiz Update(Quiz quiz)
    {
        QuizDataModel updateQuizDataModel = new QuizDataModel(quiz);
        if (_dbContext.Set<QuizDataModel>().Any(q => q.Id == quiz.Id))
        {
            var oldQuiz = ReadDataModelById(quiz.Id);
            oldQuiz.Questions = updateQuizDataModel.Questions;
            oldQuiz.Title = updateQuizDataModel.Title;
            _dbContext.SaveChanges();
            return updateQuizDataModel.MapToDomainEntity();
        }
        return null;
    }

    private QuizDataModel ReadDataModelById(Guid id)
    {
        var quiz = _dbContext.Quizes
            .Include(quiz => quiz.Questions)
                .ThenInclude(question => question.Answers)
            .Where(quiz => quiz.Id == id)
            .FirstOrDefault();
        return quiz;
    }
}