using System;
using DevPilot.Domain.Entities;
using DevPilot.Domain.ValueObjects;

namespace DevPilot.Domain;

public class Comment : BaseEntity
{
    public Content Content { get; private set; } = Content.Create("Default comment");
    public int TodoItemId { get; private set; }
    
    // Navigation properties
    public virtual TodoItem? TodoItem { get; private set; }

    // EF Core constructor
    private Comment() { }

    public Comment(Content content, int todoItemId)
    {
        Content = content;
        TodoItemId = todoItemId;
    }

    public void UpdateContent(Content content)
    {
        Content = content;
        SetUpdatedAt();
    }
} 