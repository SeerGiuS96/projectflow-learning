using FluentValidation;
using ProjectFlow.Application.Projects;
using ProjectFlow.Domain.Entities;

namespace ProjectFlow.Application.Projects.CreateProject;

public sealed class CreateProjectUseCase
{
    private readonly IProjectRepository _projectRepository;
    private readonly IValidator<CreateProjectCommand> _validator;

    public CreateProjectUseCase(IProjectRepository projectRepository, IValidator<CreateProjectCommand> validator)
    {
        _projectRepository = projectRepository;
        _validator = validator;
    }

    public async Task<CreateProjectResult> ExecuteAsync(CreateProjectCommand command, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(command, cancellationToken);

        Project project = Project.Create(command.Name, command.Description);

        await _projectRepository.AddAsync(project, cancellationToken);

        CreateProjectResult result = new(project.Id, project.Name, project.Description, project.CreatedAt);
        return result;
    }
}
