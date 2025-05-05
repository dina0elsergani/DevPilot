namespace DevPilot.Application.DTOs;

/// <summary>
/// Data Transfer Object for TodoItem.
/// </summary>
public class TodoItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ProjectId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public ProjectDto? Project { get; set; }
    public ICollection<CommentDto> Comments { get; set; } = new List<CommentDto>();
}

public class CreateTodoItemDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int ProjectId { get; set; }
    public string UserId { get; set; } = string.Empty;
}

public class UpdateTodoItemDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool? IsCompleted { get; set; }
    public int? ProjectId { get; set; }
} 