namespace Core.Dtos.Requests;

public class SigninRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}