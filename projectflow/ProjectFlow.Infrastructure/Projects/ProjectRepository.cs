using Microsoft.EntityFrameworkCore;
using ProjectFlow.Application.Projects.Ports;
using ProjectFlow.Domain.Entities;
using ProjectFlow.Infrastructure.Persistence;

namespace ProjectFlow.Infrastructure.Projects;

public sealed class ProjectRepository : IProjectRepository
{
    private readonly ProjectFlowDbContext _dbContext;

    public ProjectRepository(ProjectFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Project project, CancellationToken cancellationToken)
    {
        await _dbContext.Projects.AddAsync(project, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Project>> GetAllAsync(CancellationToken cancellationToken)
    {
        List<Project> projects = await _dbContext.Projects
            .AsNoTracking()
            .OrderByDescending(project => project.CreatedAt)
            .ToListAsync(cancellationToken);

        return projects;
    }
}
