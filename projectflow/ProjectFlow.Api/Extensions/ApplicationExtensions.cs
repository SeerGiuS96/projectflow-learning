using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProjectFlow.Application.Projects.CreateProject;

namespace ProjectFlow.Api.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddProjectFlowApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateProjectUseCase>();
        services.AddScoped<IValidator<CreateProjectCommand>, CreateProjectCommandValidator>();

        return services;
    }
}
