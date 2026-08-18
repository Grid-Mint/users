using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Users.Infrastructure.Database;

namespace Users.Infrastructure.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {


        return services;
    }

    public static IServiceCollection AddDbConnection(this IServiceCollection services, IConfiguration configuration)
    {
        var dbSettings = configuration.GetSection("DatabaseSettings");
        var dbPassword = configuration["DB:PASSWORD"]
            ?? throw new InvalidOperationException("The DB__PASSWORD environment variable is not set.");

        var connectionString = new NpgsqlConnectionStringBuilder
        {
            Host = dbSettings["DbHost"],
            Port = int.Parse(dbSettings["DbPort"] ?? "5432"),
            Database = dbSettings["DbName"],
            Username = dbSettings["DbUser"],
            Password = dbPassword
        }.ConnectionString;

        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbConnection(configuration);
        services.AddDependencyInjection(configuration);

        return services;
    }
}
