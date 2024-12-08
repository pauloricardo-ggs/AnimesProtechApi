namespace Core.v1.Auth.Dtos;

public class SigninDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}