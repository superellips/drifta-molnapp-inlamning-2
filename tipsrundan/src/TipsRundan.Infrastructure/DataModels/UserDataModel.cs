using TipsRundan.Domain.Entities;

namespace TipsRundan.Infrastructure.DataModels;

public class UserDataModel
{
    public UserDataModel(User user)
    {
        Id = user.Id;
        Username = user.Username;
        Email = user.Email;
        Password = user.PasswordHash;
    }
    public UserDataModel() { }

    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    internal User MapToDomainEntity()
    {
        return User.Load(Id, Username, Email, Password);
    }
}