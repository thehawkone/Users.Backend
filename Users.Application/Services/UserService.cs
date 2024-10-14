using Users.Application.DTOs;
using Users.DataAccess;
using Users.DataAccess.Repositories;
using Users.Domain.Models;
using Users.Application.Services;

namespace Users.Application.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly UsersDbContext _usersDbContext;
    private readonly TokenService _tokenService;

    public UserService(IUserRepository userRepository, UsersDbContext usersDbContext, TokenService tokenService)
    {
        _userRepository = userRepository;
        _usersDbContext = usersDbContext;
        _tokenService = tokenService;
    }

    public async Task<UserDto> RegisterAsync(UserRegisterDto userRegisterDto)
    {
        var passwordHash = HashPassword(userRegisterDto.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = userRegisterDto.UserName,
            PasswordHash = passwordHash,
            Email = userRegisterDto.Email
        };

        await _userRepository.CreateUserAsync(user);
        await _usersDbContext.SaveChangesAsync();
        
        return new UserDto { Id = user.Id, UserName = user.UserName, Email = user.Email };
    }
    
    public async Task<string> LoginAsync(string userName, string password)
    {
        var user = await _userRepository.GetUserByUserName(userName);
        if (user == null || !VerifyPassword(password, user.PasswordHash)) {
            throw new Exception("Пароль или логин неверный!");
        }

        await _usersDbContext.SaveChangesAsync();
        return _tokenService.GenerateJwtToken(userName);
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
    {
        var user = await _userRepository.GetUserById(userId);
        if (user == null) {
            throw new Exception("Пользователь не найден!");
        }

        if (!VerifyPassword(currentPassword, user.PasswordHash)) {
            throw new Exception("Пароль неверный!");
        }
        
        var newHashedPassword = HashPassword(newPassword);
        user.PasswordHash = newHashedPassword;
        
        await _userRepository.UpdateUserAsync(user);
        await _usersDbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        var user = await _userRepository.GetUserById(userId);
        if (user == null) {
            throw new Exception("Пароль неверный!");
        }
        
        await _userRepository.DeleteUserAsync(userId);
        await _usersDbContext.SaveChangesAsync();

        return true;
    }
    
    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password); 
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}