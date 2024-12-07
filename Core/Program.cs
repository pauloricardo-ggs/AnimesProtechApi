using Core.Configurations;
using Domain.Constants;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
ConfigurationEnvironment.Configure(builder.Configuration);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddDatabase();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddIdentity();
builder.Services.AddJwtAuthentication();
builder.Services.InjectServices();

var app = builder.Build();
app.UseSwaggerIfIsDevelopment();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
