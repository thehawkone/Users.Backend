using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Users.DataAccess;
using Users.DataAccess.Repositories;

namespace Users.Application.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly UsersDbContext _usersDbContext;

    public UserService(IUserRepository userRepository, UsersDbContext usersDbContext)
    {
        _userRepository = userRepository;
        _usersDbContext = usersDbContext;
    }

    public async Task<IActionResult> RegisterAsync(string userName, string password, string email)
    {
        if (await _usersDbContext.Users.AnyAsync(u => u.UserName == userName))
        {
            return BadRequest("Данный пользователь уже зарегистрирован");
        }
        
        return null;
    }
}