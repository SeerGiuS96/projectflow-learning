using ProjectFlow.Domain.Entities;

namespace ProjectFlow.Application.Projects.Ports;

public interface IProjectRepository
{
    Task AddAsync(Project project, CancellationToken cancellationToken);

    Task<IReadOnlyList<Project>> GetAllAsync(CancellationToken cancellationToken);
}
