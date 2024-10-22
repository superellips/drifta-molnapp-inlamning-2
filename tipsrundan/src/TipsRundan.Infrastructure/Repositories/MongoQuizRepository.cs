using MongoDB.Driver;
using TipsRundan.Domain.Entities;
using TipsRundan.Domain.Interfaces;
using TipsRundan.Infrastructure.DataModels;

namespace TipsRundan.Infrastructure.Repositories;

public class MongoQuizRepository : IQuizRepository

{
    private readonly IMongoCollection<QuizDataModel> _quizzes;

    public MongoQuizRepository(IMongoDatabase database)
    {
        _quizzes = database.GetCollection<QuizDataModel>("quizzes");
    }

    public Quiz Create(Quiz quiz)
    {
        var dataModel = new QuizDataModel(quiz);
        _quizzes.InsertOne(dataModel);
        return dataModel.MapToDomainEntity();
    }

    public Quiz Delete(Quiz quiz)
    {
        throw new NotImplementedException();
    }

    public List<Quiz> ReadAll()
    {
        var dataModels = _quizzes.Find(q => true).ToList();
        return dataModels.Select(q => q.MapToDomainEntity()).ToList();
    }

    public Quiz ReadById(Guid id)
    {
        var dataModel = _quizzes.Find(q => q.Id == id).FirstOrDefault();
        return dataModel.MapToDomainEntity();
    }

    public Quiz Update(Quiz quiz)
    {
        throw new NotImplementedException();
    }
    
}