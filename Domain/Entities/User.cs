using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : IdentityUser<Guid>
{
    public User(Guid id, string email, bool emailConfirmed)
    {
        Id = id;
        UserName = email;
        Email = email;
        EmailConfirmed = emailConfirmed;
    }
}