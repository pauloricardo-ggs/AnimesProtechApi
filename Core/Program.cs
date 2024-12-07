using Core.Configurations;
using Domain.Constants;
using DotNetEnv;
using MediatR;

var builder = WebApplication.CreateBuilder(args);
var assembly = AppDomain.CurrentDomain.Load("Application");

Env.Load();
ConfigurationEnvironment.Configure(builder.Configuration);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddMediatR(assembly);
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
builder.Services.AddVersioning();
builder.Services.AddDatabase();
builder.Services.AddControllersWithRoutePrefix("api/v{version:apiVersion}");
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
