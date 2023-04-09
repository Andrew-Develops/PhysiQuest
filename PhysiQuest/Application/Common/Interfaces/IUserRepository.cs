using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetUsersByPointsDescendingAsync();
        Task<User> GetUserByNameAsync(string userName);
    }

}
