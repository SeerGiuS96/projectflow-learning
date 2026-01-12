using ProjectFlow.Application.Projects.Queries.GetProjects;

namespace ProjectFlow.Api.Endpoints;

public static class ProjectsEndpoints
{
    public static IEndpointRouteBuilder MapProjectsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        RouteGroupBuilder group = endpoints
            .MapGroup("/projects")
            .RequireAuthorization();

        group.MapGet("/", async (
            GetProjectsHandler handler,
            CancellationToken cancellationToken) =>
        {
            GetProjectsQuery query = new GetProjectsQuery();

            IReadOnlyList<ProjectSummaryDto> projects = await handler.HandleAsync(query, cancellationToken);

            return Results.Ok(projects);
        });

        return endpoints;
    }
}
