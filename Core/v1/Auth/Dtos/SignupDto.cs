namespace Core.v1.Auth.Dtos;

public class SignupDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string PasswordConfirmation { get; set; }
}