using TipsRundan.Domain.Entities;

namespace TipsRundan.Application.Interfaces;

public interface IAuthnService
{
    void SigninUser(string userName, string passwordString);
    void SignoutUser();
    User GetCurrentUser();
}