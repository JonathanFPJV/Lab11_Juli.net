namespace Lab11_Juli.Application.DTOs;

public class RegisterUserDto
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? Email { get; set; }
    public string? RoleName { get; set; } = "User"; // Rol por defecto
}

public class LoginUserDto
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class AuthResponseDto
{
    public string Token { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Role { get; set; } = null!;
}