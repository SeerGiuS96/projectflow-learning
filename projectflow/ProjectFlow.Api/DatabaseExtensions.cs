using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectFlow.Infrastructure.Persistence;

namespace ProjectFlow.Api.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddProjectFlowDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string provider = configuration["Database:Provider"] ?? "Sqlite";
        string connectionStringName = provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase)
            ? "ProjectFlowDbSqlServer"
            : "ProjectFlowDbSqlite";
        string connectionString = configuration.GetConnectionString(connectionStringName)!;

        services.AddDbContext<ProjectFlowDbContext>(options =>
        {
            if (provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
            {
                options.UseSqlServer(connectionString);
                return;
            }

            options.UseSqlite(connectionString);
        });

        return services;
    }
}
