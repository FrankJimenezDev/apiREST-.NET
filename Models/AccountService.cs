using Common.Entities;
using Common.Interfaces;
using DBContext;
using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class AccountService : IAccountService
    {

        private readonly AppDbContext _context;

        public AccountService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Account>> GetAllAccountsAsync()
        {
            try
            {
                return await _context.Accounts
                             .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while retrieving users: {ex.Message}");
                throw; // Re-throw the exception after logging it
            }
        }

        public async Task<List<Account>> GetAccountsByUserIdAsync(int userId)
        {
            try
            {
                return await _context.Accounts
                     .Where(a => a.UserId == userId)  // ✅ Filtro por userId
                     .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while retrieving users: {ex.Message}");
                throw; // Re-throw the exception after logging it
            }
        }

        public async Task<decimal> GetBalanceOfAnAccountAsync(int accountId)
        {
            try
            {
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);

                if (account == null)
                    throw new Exception($"Account with ID {accountId} not found");

                return account.Balance;  // ✅ Directamente la propiedad Balance
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while retrieving users: {ex.Message}");
                throw; // Re-throw the exception after logging it
            }
        }
    }
}