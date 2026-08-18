using Microsoft.EntityFrameworkCore;
using Serilog;
using Users.Api.Utils;
using Users.Infrastructure.Database;
using Users.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(((context, configuration) => configuration
        .WriteTo.Console()
        .WriteTo.File("logs/log-.log", rollingInterval: RollingInterval.Day)
        .ReadFrom.Configuration(context.Configuration)
    ));

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
}

app.UseExceptionHandler();
app.UseSerilogRequestLogging();

app.MapGet("/users", () => "[users]");
app.MapGet("/user/{id}", (int id) => $"user {id}");


app.Run();
