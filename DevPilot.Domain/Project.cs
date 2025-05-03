using System;
using System.Collections.Generic;
using DevPilot.Domain.ValueObjects;
using DevPilot.Domain.Entities;

namespace DevPilot.Domain;

public class Project : BaseEntity
{
    public Name Name { get; private set; } = Name.Create("Untitled Project");
    public Description? Description { get; private set; }
    public UserId UserId { get; private set; } = UserId.Create("system@example.com");
    
    // Navigation properties
    public virtual ICollection<TodoItem> TodoItems { get; private set; } = new List<TodoItem>();

    // EF Core constructor
    private Project() { }

    public Project(Name name, UserId userId, Description? description = null)
    {
        Name = name;
        UserId = userId;
        Description = description;
    }

    public void UpdateName(Name name)
    {
        Name = name;
        SetUpdatedAt();
    }

    public void UpdateDescription(Description? description)
    {
        Description = description;
        SetUpdatedAt();
    }

    public void AddTodoItem(TodoItem todoItem)
    {
        TodoItems.Add(todoItem);
        SetUpdatedAt();
    }

    public void RemoveTodoItem(TodoItem todoItem)
    {
        TodoItems.Remove(todoItem);
        SetUpdatedAt();
    }
} 