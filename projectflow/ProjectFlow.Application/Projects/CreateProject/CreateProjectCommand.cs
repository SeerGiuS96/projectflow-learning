namespace ProjectFlow.Application.Projects.CreateProject;

public sealed class CreateProjectCommand
{
    public CreateProjectCommand(string name, string? description)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; }

    public string? Description { get; }
}
