namespace ProjectFlow.Application.Projects.CreateProject;

public sealed class CreateProjectResult
{
    public CreateProjectResult(Guid id, string name, string? description, DateTime createdAt)
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
