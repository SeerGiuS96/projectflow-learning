using System;

namespace ProjectFlow.Domain.Entities;

public class Project
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Project(Guid id, string name, string? description = null, DateTime? createdAt = null)
    {
        Id = id == Guid.Empty ? throw new ArgumentException("Project id cannot be empty.", nameof(id)) : id;
        Name = ValidateName(name);
        Description = description;
        CreatedAt = createdAt ?? DateTime.UtcNow;
    }

    public void Rename(string name)
    {
        Name = ValidateName(name);
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
    }

    private static string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Project name cannot be empty.", nameof(name));
        }

        var trimmed = name.Trim();

        if (trimmed.Length > 200)
        {
            throw new ArgumentException("Project name is too long.", nameof(name));
        }

        return trimmed;
    }
}
