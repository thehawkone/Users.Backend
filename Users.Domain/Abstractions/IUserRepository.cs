using Users.Domain.Models;

namespace Users.DataAccess.Repositories;

public interface IUserRepository
{
    Task<User> GetUserById(Guid id);
    Task<User> GetUserByUserName(string email);
    Task CreateUserAsync(User user);
    Task DeleteUserAsync(Guid id);
    Task UpdateUserAsync(User user);
}