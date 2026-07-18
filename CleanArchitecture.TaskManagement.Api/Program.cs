using CleanArchitecture.TaskManagement.Application;
using CleanArchitecture.TaskManagement.Infrastructure;
using CleanArchitecture.TaskManagement.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Seed demo data (best effort). Do not throw on failure in production scenarios.
try
{
    await DataSeeder.SeedAsync(app.Services);
}
catch (Exception ex)
{
    // Log the error in real apps. Keep startup resilient in dev/demo scenarios.
    Console.WriteLine($"Data seeding failed: {ex.Message}");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program;
