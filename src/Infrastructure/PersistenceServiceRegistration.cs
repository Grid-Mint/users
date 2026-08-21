using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Users.Application.Abstractions;
using Users.Application.Users.Commands.CreateUser;
using Users.Domain.Repositories;
using Users.Infrastructure.Database;
using Users.Infrastructure.Database.Repositories;
using Users.Infrastructure.Security;

namespace Users.Infrastructure;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<CreateUserCommandHandler>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();

        return services;
    }

    public static IServiceCollection AddDbConnection(this IServiceCollection services, IConfiguration configuration)
    {
        var dbPassword = configuration["DB:PASSWORD"]
            ?? throw new InvalidOperationException("The DB__PASSWORD environment variable is not set.");
        var dbSettings = configuration.GetSection("DB");
        var usersDbSettings = configuration.GetSection("USERS:DB");

        var connectionString = new NpgsqlConnectionStringBuilder
        {
            Host = usersDbSettings["HOST"] ?? "localhost",
            Port = int.Parse(dbSettings["PORT"] ?? "5432"),
            Database = usersDbSettings["NAME"],
            Username = dbSettings["USER"],
            Password = dbPassword
        }.ConnectionString;

        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbConnection(configuration);
        services.AddValidators(configuration);
        services.AddDependencyInjection(configuration);

        return services;
    }
}
