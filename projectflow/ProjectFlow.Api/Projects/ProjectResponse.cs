namespace ProjectFlow.Api.Projects;

public sealed class ProjectResponse
{
    public ProjectResponse(Guid id, string name, string? description, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Description = description;
        CreatedAt = createdAt;
    }

    public Guid Id { get; }

    public string Name { get; }

    public string? Description { get; }

    public DateTime CreatedAt { get; }
}
