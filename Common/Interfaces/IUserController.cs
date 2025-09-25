using Common.Entities;

namespace Common.Interfaces
{
    public interface IUserController
    {
        public Task<List<User>> GetAllUsers();
    }
}
