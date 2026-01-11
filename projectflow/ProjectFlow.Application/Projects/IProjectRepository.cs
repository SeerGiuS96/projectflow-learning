using ProjectFlow.Domain.Entities;

namespace ProjectFlow.Application.Projects;

public interface IProjectRepository
{
    Task AddAsync(Project project, CancellationToken cancellationToken);
}
