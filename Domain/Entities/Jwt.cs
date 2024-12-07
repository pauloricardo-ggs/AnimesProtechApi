namespace Domain.Entities;

public class Jwt
{
    public required string Token { get; set; }
    public required string ExpiresIn { get; set; }
}