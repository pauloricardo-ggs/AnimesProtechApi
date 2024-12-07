using Asp.Versioning;

namespace Core.Configurations;

public static class VersioningConfig
{
    public static void AddVersioning(this IServiceCollection service)
    {
        service.AddApiVersioning(static options => 
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
    }
}