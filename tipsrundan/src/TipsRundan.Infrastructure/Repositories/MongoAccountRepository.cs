using MongoDB.Driver;
using TipsRundan.Domain.Entities;
using TipsRundan.Domain.Interfaces;
using TipsRundan.Infrastructure.DataModels;

namespace TipsRundan.Infrastructure.Repositories
{
    public class MongoAccountRepository : IAccountRepository
    {
     private readonly IMongoCollection<UserDataModel> _users;
     public MongoAccountRepository(IMongoDatabase database)
     {
        _users = database.GetCollection<UserDataModel>("users");
     }

     public User Create(User user)
     {
        var dataModel = new UserDataModel(user);
        _users.InsertOne(dataModel);
        return dataModel.MapToDomainEntity();
     }

     public User Delete(User user)
     {
        var deleteResult = _users.DeleteOne(u => u.Id == user.Id);
        if (deleteResult.DeletedCount > 0)
        {
            return user;
        }
        return null;
     }

     public List<User> ReadAll()
     {
        var dataModels = _users.Find(a => true).ToList();
        return dataModels.Select(u => u.MapToDomainEntity()).ToList();
     }

     public User ReadById(Guid id)
     {
        var dataModel = _users.Find(u => u.Id == id).FirstOrDefault();
        return dataModel.MapToDomainEntity();
     }

        public User ReadByName(string userName)
        {
            try
            {
               var dataModel = _users.Find(u => u.Username == userName).FirstOrDefault();
               return dataModel.MapToDomainEntity();
            }
            catch
            {
               return null;
            }
        }

        public User Update(User user)
     {
        var updateDataModel = new UserDataModel(user);
        var replaceResult = _users.ReplaceOne(u => u.Id ==user.Id, updateDataModel);
        if (replaceResult.ModifiedCount > 0)
        {
            return updateDataModel.MapToDomainEntity();
        }
        return null;
     }

    }
}