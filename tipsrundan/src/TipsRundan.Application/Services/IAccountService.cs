using TipsRundan.Application.DataTransferObjects;

namespace TipsRundan.Application.Services
{
    public interface IAccountService
    {
        Task<bool> CreateAccountAsync(CreateAccountDTO accountDto);
        Task<AccountDTO> GetAccountByIdAsync(Guid accountId);
        Task<bool> UpdateAccountAsync(UpdateAccountDTO accountDto);
        Task<bool> DeleteAccountAsync(Guid accountId);
        Task SigninUserAsync(LoginDTO loginInformation);
        Task SignoutUserAsync();
    }
}