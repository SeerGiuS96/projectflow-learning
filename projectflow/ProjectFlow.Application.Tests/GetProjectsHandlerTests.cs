using FluentAssertions;
using Moq;
using ProjectFlow.Application.Projects.Ports;
using ProjectFlow.Application.Projects.Queries.GetProjects;
using ProjectFlow.Domain.Entities;
using Xunit;

namespace ProjectFlow.Application.Tests.Projects.Queries.GetProjects;

public sealed class GetProjectsHandlerTests
{
    [Fact]
    public async Task HandleAsync_WhenRepositoryReturnsEmptyList_ReturnsEmptyList()
    {
        Mock<IProjectRepository> projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);

        projectRepositoryMock
            .Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<Project>());

        GetProjectsHandler handler = new GetProjectsHandler(projectRepositoryMock.Object);
        GetProjectsQuery query = new GetProjectsQuery();

        IReadOnlyList<ProjectSummaryDto> result = await handler.HandleAsync(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEmpty();

        projectRepositoryMock.Verify(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        projectRepositoryMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task HandleAsync_WhenRepositoryReturnsProjects_MapsToDtos()
    {
        Mock<IProjectRepository> projectRepositoryMock = new Mock<IProjectRepository>(MockBehavior.Strict);

        Project project1 = Project.Create("Project A");
        Project project2 = Project.Create("Project B");

        IReadOnlyList<Project> projects = new List<Project> { project1, project2 };

        projectRepositoryMock
            .Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(projects);

        GetProjectsHandler handler = new GetProjectsHandler(projectRepositoryMock.Object);
        GetProjectsQuery query = new GetProjectsQuery();

        IReadOnlyList<ProjectSummaryDto> result = await handler.HandleAsync(query, CancellationToken.None);

        result.Should().HaveCount(2);

        result[0].Id.Should().Be(project1.Id);
        result[0].Name.Should().Be(project1.Name);
        result[0].CreatedAtUtc.Should().Be(project1.CreatedAt);

        result[1].Id.Should().Be(project2.Id);
        result[1].Name.Should().Be(project2.Name);
        result[1].CreatedAtUtc.Should().Be(project2.CreatedAt);
        projectRepositoryMock.Verify(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        projectRepositoryMock.VerifyNoOtherCalls();
    }
}
