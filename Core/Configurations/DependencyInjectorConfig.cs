using Core.Helpers;
using DataAccess.Contexts;
using Domain.Interfaces;

namespace Core.Configurations;

public static class DependencyInjectorConfig
{
    public static void InjectServices(this IServiceCollection service)
    {
        service.AddScoped<ApplicationDbContext>();

        // repositories
        service.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
    }
}
