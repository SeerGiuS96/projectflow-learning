using Microsoft.Extensions.DependencyInjection;
using ProjectFlow.Application.Projects;
using ProjectFlow.Infrastructure.Projects;

namespace ProjectFlow.Api.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddProjectFlowInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IProjectRepository, ProjectRepository>();
        return services;
    }
}
