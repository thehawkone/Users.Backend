namespace Users.Application.DTOs;

public class ChangePasswordDto : UserLoginDto
{
    public string newPassword { get; set; }
}