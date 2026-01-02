using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectFlow.Infrastructure.Persistence;

namespace ProjectFlow.Api.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddProjectFlowDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ProjectFlowDbContext>(options =>
            options.UseSqlite(connectionString));

        return services;
    }
}
