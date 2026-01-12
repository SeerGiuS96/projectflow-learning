namespace ProjectFlow.Application.Projects.Queries.GetProjects;

public sealed record ProjectSummaryDto(
    Guid Id,
    string Name,
    DateTime CreatedAtUtc
);
