using Microsoft.OpenApi.Models;

namespace Core.Configurations;

public static class SwaggerConfig
{
    public static void AddSwagger(this IServiceCollection service)
    {
        service.AddEndpointsApiExplorer();
        service.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Insert JWT token in the format: Bearer {token}",
            });
            
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "API Documentation",
            Version = "v1",
            Description = "This API uses JWT for authentication.",
        });
        });
    }
    
    public static void UseSwaggerIfIsDevelopment(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            /// No .net 9 o swagger deixou de ser usado como padrão por falta
            /// de manutenção e release oficial para o .net 8.
            /// O recomendado atualmente é o Scalar, que também suporta o OpenApi.
            /// Para usar o Scalar é só adicionar o pacote Scalar.AspNetCore e
            /// usar o metodo MapScalarApiReference() ao invés do UseSwaggerUI().
        }
    }
}