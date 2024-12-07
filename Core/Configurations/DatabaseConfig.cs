using DataAccess.Contexts;
using Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Core.Configurations;

public static class DatabaseConfig
{
    public static void AddDatabase(this IServiceCollection service)
    {
        var connectionString = GetConnectionString();
        service.AddDbContext<ApplicationDbContext>(options => 
        {

            options.UseNpgsql(connectionString, npgsqlOptionsAction => npgsqlOptionsAction.UseNodaTime());
            options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning)); // Ignorar erro que está ocorrendo no EF 9
        });
    }

    private static string GetConnectionString()
    {
        var server = ConfigurationEnvironment.GetEnvironmentVariable("CONNECTION_STRING_SERVER");
        var port = ConfigurationEnvironment.GetEnvironmentVariable("CONNECTION_STRING_PORT");
        var database = ConfigurationEnvironment.GetEnvironmentVariable("CONNECTION_STRING_DATABASE");
        var username = ConfigurationEnvironment.GetEnvironmentVariable("CONNECTION_STRING_USERNAME");
        var password = ConfigurationEnvironment.GetEnvironmentVariable("CONNECTION_STRING_PASSWORD");

        return $"Server={server};Port={port};Database={database};Username={username};Password={password}";
    }
}
