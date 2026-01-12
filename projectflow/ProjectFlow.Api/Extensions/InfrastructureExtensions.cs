
using ProjectFlow.Application.Projects.Ports;
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
