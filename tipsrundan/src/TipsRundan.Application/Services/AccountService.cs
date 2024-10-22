using TipsRundan.Application.DataTransferObjects;
using TipsRundan.Application.Interfaces;
using TipsRundan.Domain.Entities;
using TipsRundan.Domain.Interfaces;

namespace TipsRundan.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _repository;
    private readonly IAuthnService _authn;
    private readonly ICryptographyService _security;
    public AccountService(IAccountRepository repository, ICryptographyService security, IAuthnService authn)
    {
        _repository = repository;
        _security = security;
        _authn = authn;
    }
    public async Task<bool> CreateAccountAsync(CreateAccountDTO accountDto)
    {
        try
        {
            var passwordHash = _security.HashPassword(accountDto.password);
            var userWithName = _repository.ReadByName(accountDto.username);
            if (userWithName is null)
            {
                _repository.Create(User.Create(accountDto.username, accountDto.email, passwordHash));
                return true;
            }
            else 
            {
                throw new Exception("User with that name already exists.");
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<bool> DeleteAccountAsync(Guid accountId)
    {
        try
        {
            var user = _repository.ReadById(accountId);
            _repository.Delete(user);
            return true;
        }
        catch
        {
            throw;
        }
    }

    public async Task<AccountDTO> GetAccountByIdAsync(Guid accountId)
    {
        try
        {
            var account = _repository.ReadById(accountId);
            return new AccountDTO(){
                username = account.Username,
                AccountId = account.Id,
                email = account.Email
            };
        }
        catch
        {
            throw;
        }
    }

    public async Task SigninUserAsync(LoginDTO loginInformation)
    {
        try
        {
            _authn.SigninUser(loginInformation.Name, loginInformation.PasswordString);
        }
        catch
        {
            throw;
        }
    }

    public async Task SignoutUserAsync()
    {
        try
        {
            _authn.SignoutUser();
        }
        catch
        {
            throw;
        }
    }

    public Task<bool> UpdateAccountAsync(UpdateAccountDTO accountDto)
    {
        throw new NotImplementedException();
    }
}