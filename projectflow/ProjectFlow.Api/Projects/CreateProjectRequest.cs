namespace ProjectFlow.Api.Projects;

public sealed class CreateProjectRequest
{
    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }
}
