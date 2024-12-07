using Application.Queries;
using Core.Helpers;
using DataAccess.Contexts;
using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;

namespace Core.Configurations;

public static class DependencyInjectorConfig
{
    public static void InjectServices(this IServiceCollection service)
    {
        service.AddScoped<ApplicationDbContext>();

        service.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        
        #region Queries
        service.AddScoped<IAnimeQueries, AnimeQueries>();
        #endregion

        #region Repositories
        service.AddScoped<IAnimeRepository, AnimeRepository>();
        #endregion
    }
}
