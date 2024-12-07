using Microsoft.Extensions.Configuration;

namespace Domain.Constants;

public static class ConfigurationEnvironment
{
    private static IConfiguration? Configuration;

    public static void Configure(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public static string GetEnvironmentVariable(string variableName)
    {
        return Configuration?[variableName]!;
    }
}
