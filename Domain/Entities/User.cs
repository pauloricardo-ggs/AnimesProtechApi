using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : IdentityUser<Guid>
{
    public User(Guid id, string email, bool emailConfirmed)
    {
        Id = id;
        Email = email;
        NormalizedEmail = email.ToUpper();
        UserName = email;
        NormalizedUserName = email.ToUpper();
        EmailConfirmed = emailConfirmed;
    }

    public User(string email)
    {
        UserName = email;
        Email = email;
    }
}