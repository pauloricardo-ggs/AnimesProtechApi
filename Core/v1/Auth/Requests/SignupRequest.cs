namespace Core.Dtos.Requests;

public class SignupRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string PasswordConfirmation { get; set; }
}