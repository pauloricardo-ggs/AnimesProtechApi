using Core.Configurations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();
app.UseSwaggerIfIsDevelopment();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
