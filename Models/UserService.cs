using Common.Entities;
using Common.Interfaces;
using DBContext;
using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class UserService : IUserService
    {

        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(IUser user)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            try { 
                return await _context.Users
                             .Include(u => u.Accounts)
                             .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while retrieving users: {ex.Message}");
                throw; // Re-throw the exception after logging it
            }
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            try
            {
                
                var user = await _context.Users.FirstOrDefaultAsync(a => a.Id == userId);  // ✅ Filtro por
                if (user == null)
                    throw new Exception($"User with ID {userId} not found");

                return user;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while retrieving user: {ex.Message}");
                throw; // Re-throw the exception after logging it
            }
        }

        public async Task UpdateUserAsync(IUser user)
        {
            throw new NotImplementedException();
        }
    }
}
