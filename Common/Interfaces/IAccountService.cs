using Common.Entities;

namespace Common.Interfaces
{
    public interface IAccountService
    {

        Task<List<Account>> GetAllAccountsAsync();

        Task<List<Account>> GetAccountsByUserIdAsync(int userId);

        Task<decimal> GetBalanceOfAnAccountAsync(int accountId);
    }
}
