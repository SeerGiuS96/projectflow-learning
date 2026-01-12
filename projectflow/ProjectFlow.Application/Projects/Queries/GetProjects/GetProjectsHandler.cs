using ProjectFlow.Application.Projects.Ports;
using ProjectFlow.Domain.Entities;

namespace ProjectFlow.Application.Projects.Queries.GetProjects;

public sealed class GetProjectsHandler
{
    private readonly IProjectRepository _projectRepository;

    public GetProjectsHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<IReadOnlyList<ProjectSummaryDto>> HandleAsync(
        GetProjectsQuery query,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<Project> projects = await _projectRepository.GetAllAsync(cancellationToken);

        List<ProjectSummaryDto> result = projects
            .Select(project => new ProjectSummaryDto(
                project.Id,
                project.Name,
                project.CreatedAt))
            .ToList();

        return result;
    }
}

