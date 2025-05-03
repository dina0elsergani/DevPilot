using DevPilot.Domain.Entities;
using DevPilot.Domain.Events;
using DevPilot.Domain.ValueObjects;

namespace DevPilot.Domain;

/// <summary>
/// Represents a to-do item in the system.
/// </summary>
public class TodoItem : BaseEntity
{
    public Title Title { get; private set; } = Title.Create("Untitled");
    public Description? Description { get; private set; }
    public bool IsCompleted { get; private set; }
    public int ProjectId { get; private set; }
    public UserId UserId { get; private set; } = UserId.Create("default@example.com");
    
    // Navigation properties
    public virtual Project? Project { get; private set; }
    public virtual ICollection<Comment> Comments { get; private set; } = new List<Comment>();

    // EF Core constructor
    private TodoItem() { }

    public TodoItem(Title title, int projectId, UserId userId, Description? description = null)
    {
        Title = title;
        ProjectId = projectId;
        UserId = userId;
        Description = description;
        IsCompleted = false;
    }

    public void UpdateTitle(Title title)
    {
        Title = title;
        SetUpdatedAt();
    }

    public void UpdateDescription(Description? description)
    {
        Description = description;
        SetUpdatedAt();
    }

    public void MarkAsCompleted()
    {
        if (IsCompleted)
            return;
        
        IsCompleted = true;
        SetUpdatedAt();
        AddDomainEvent(new TodoCompletedEvent(Id, UserId.Value));
    }

    public void MarkAsIncomplete()
    {
        if (!IsCompleted)
            return;
        
        IsCompleted = false;
        SetUpdatedAt();
    }

    public void UpdateProject(int projectId)
    {
        ProjectId = projectId;
        SetUpdatedAt();
    }

    public void AddComment(Comment comment)
    {
        Comments.Add(comment);
        SetUpdatedAt();
    }

    public void RemoveComment(Comment comment)
    {
        Comments.Remove(comment);
        SetUpdatedAt();
    }

    public void Delete()
    {
        AddDomainEvent(new TodoDeletedEvent(Id, UserId.Value));
    }
} 