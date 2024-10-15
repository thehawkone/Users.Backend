using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Quic;
using Users.Application.DTOs;
using Users.Application.Services;

namespace Users.API.Controllers;


[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;
    private readonly TokenService _tokenService;

    public AuthController(UserService userService, TokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
    {
        var user = await _userService.RegisterAsync(userRegisterDto);
        return Ok("Пользователь успешно зарегистрирован!");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
    {
        var user = await _userService.LoginAsync(userLoginDto.UserName, userLoginDto.Password);
        if (user == null) {
            return Unauthorized("Пароль или логин неверный!");
        }
        var token = _tokenService.GenerateJwtToken(userLoginDto.UserName);
        return Ok($"Авторизация прошла успешно, ваш токен: {token}");
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword(Guid userId, ChangePasswordDto changePasswordDto)
    {
        await _userService.ChangePasswordAsync
            (userId, changePasswordDto.Password, changePasswordDto.newPassword);

        return Ok("Пароль успешно изменён!");
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try {
            await _userService.DeleteUserAsync(id);
            return Ok("Пользователь удалён!");
        }
        catch (ArgumentException ex) {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex) {
            return NotFound(ex.Message);
        }
    }
}