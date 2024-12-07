using DataAccess.Contexts;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Core.Configurations;

public static class IdentityConfig
{
    public static void AddIdentity(this IServiceCollection service)
    {
        service.AddIdentity<User, IdentityRole<Guid>>(options =>
        {
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
    }
}
