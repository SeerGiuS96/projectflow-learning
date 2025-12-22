using Microsoft.EntityFrameworkCore;
using ProjectFlow.Domain.Entities;

namespace ProjectFlow.Infrastructure.Persistence;

public sealed class ProjectFlowDbContext : DbContext
{
    public ProjectFlowDbContext(DbContextOptions<ProjectFlowDbContext> options)
        : base(options)
    {
    }

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<WorkItem> WorkItems => Set<WorkItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProjectFlowDbContext).Assembly);
    }
}
