using Common.Entities;
using Common.Interfaces;

public interface IUserService
{
    Task<List<User>> GetAllUsersAsync();
    Task<User> GetUserByIdAsync(int id);
    Task AddUserAsync(IUser user);
    Task UpdateUserAsync(IUser user);
    Task DeleteUserAsync(int id);
}
