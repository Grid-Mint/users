using Microsoft.EntityFrameworkCore;
using Serilog;
using Users.Api.Utils;
using Users.Infrastructure;
using Users.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(((context, configuration) => configuration
        .WriteTo.Console()
        .WriteTo.File("logs/log-.log", rollingInterval: RollingInterval.Day)
        .ReadFrom.Configuration(context.Configuration)
    ));

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
}

app.UseExceptionHandler();
app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();
