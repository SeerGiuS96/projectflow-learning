using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ProjectFlow.Application.Projects.CreateProject;

namespace ProjectFlow.Api.Projects;

public static class ProjectEndpoints
{
    public static IEndpointRouteBuilder MapProjectEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/projects", async (
            CreateProjectRequest request,
            CreateProjectUseCase useCase,
            CancellationToken cancellationToken) =>
        {
            CreateProjectCommand command = new(request.Name, request.Description);
            CreateProjectResult result = await useCase.ExecuteAsync(command, cancellationToken);

            ProjectResponse response = new(result.Id, result.Name, result.Description, result.CreatedAt);

            return Results.Created($"/projects/{response.Id}", response);
        }).RequireAuthorization();

        return endpoints;
    }
}
