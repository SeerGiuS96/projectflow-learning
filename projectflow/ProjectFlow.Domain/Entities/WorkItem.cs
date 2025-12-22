using System;
using ProjectFlow.Domain.Enums;

namespace ProjectFlow.Domain.Entities;

public class WorkItem
{
    public Guid Id { get; private set; }
    public Guid ProjectId { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public WorkItemStatus Status { get; private set; }
    public WorkItemPriority Priority { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    private WorkItem()
    {
        Title = string.Empty;
    }

    public WorkItem(Guid id, Guid projectId, string title, string? description = null)
    {
        Id = id == Guid.Empty
            ? throw new ArgumentException("WorkItem id cannot be empty.", nameof(id))
            : id;

        ProjectId = projectId == Guid.Empty
            ? throw new ArgumentException("ProjectId cannot be empty.", nameof(projectId))
            : projectId;

        Title = ValidateTitle(title);
        Description = description;

        Status = WorkItemStatus.Todo;
        Priority = WorkItemPriority.Medium;
        CreatedAt = DateTime.UtcNow;
    }

    public void Rename(string title)
    {
        Title = ValidateTitle(title);
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
    }

    private static string ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("WorkItem title cannot be empty.", nameof(title));
        }

        var trimmed = title.Trim();

        if (trimmed.Length > 200)
        {
            throw new ArgumentException("WorkItem title is too long.", nameof(title));
        }

        return trimmed;
    }

    public void ChangeStatus(WorkItemStatus newStatus){
        Status = newStatus;
        if (newStatus == WorkItemStatus.Done)
        {
            CompletedAt = DateTime.UtcNow;
        }
        else
        {
            CompletedAt = null;
        }
    }

    public void MarkAsDone(){
        ChangeStatus(WorkItemStatus.Done);
    }

}
