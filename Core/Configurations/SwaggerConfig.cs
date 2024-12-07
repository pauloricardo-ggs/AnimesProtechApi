namespace Core.Configurations;

public static class SwaggerConfig
{
    public static void UseSwaggerIfIsDevelopment(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "v1"));
            /// No .net 9 o swagger deixou de ser usado como padrão por falta
            /// de manutenção e release oficial para o .net 8.
            /// O recomendado atualmente é o Scalar, que também suporta o OpenApi.
            /// Para usar o Scalar é só adicionar o pacote Scalar.AspNetCore e
            /// usar o metodo MapScalarApiReference() ao invés do UseSwaggerUI().
        }
    }
}