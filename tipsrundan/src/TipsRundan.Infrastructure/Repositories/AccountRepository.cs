using TipsRundan.Domain.Entities;
using TipsRundan.Domain.Interfaces;
using TipsRundan.Infrastructure.DataModels;
using TipsRundan.Infrastructure.DbContexts;

namespace Tipsrundan.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly SqliteDbContext _dbContext;
        public AccountRepository(SqliteDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User Create(User user)
        {
            var dataModelUser = new UserDataModel(user);
            dataModelUser.Id = _dbContext.Add(dataModelUser).CurrentValues.GetValue<Guid>(nameof(UserDataModel.Id));
            _dbContext.SaveChanges();
            return dataModelUser.MapToDomainEntity();
        }

        public User Delete(User user)
        {
            var deleteUserDataModel = new UserDataModel(user);
            if (_dbContext.Set<UserDataModel>().Any(u => u.Id == user.Id))
            {
                _dbContext.Remove(deleteUserDataModel);
                _dbContext.SaveChanges();
                return deleteUserDataModel.MapToDomainEntity();

            }
            return null;
        }

        public List<User> ReadAll()
        {
            var dataModelUser = _dbContext.Users.ToList();
            return dataModelUser.Select( u => u.MapToDomainEntity()).ToList();
        }

        public User ReadById(Guid id)
        {
            var dataModelUser = ReadDataModelById(id);
            return dataModelUser?.MapToDomainEntity();
        }

        public User ReadByName(string userName)
        {
            throw new NotImplementedException();
        }

        public User Update(User user)
        {
            var updateUserDataModel = new UserDataModel(user);
            if (_dbContext.Set<UserDataModel>().Any(u => u.Id == user.Id))
            {
                var oldUser = ReadDataModelById(user.Id);
                oldUser.Username = updateUserDataModel.Username;
                oldUser.Password = updateUserDataModel.Password;
                _dbContext.SaveChanges();
                return updateUserDataModel.MapToDomainEntity();
            }
            return null;

        }

        private UserDataModel ReadDataModelById(Guid id)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Id == id);
        }
    }
}