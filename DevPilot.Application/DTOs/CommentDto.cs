using System;

namespace DevPilot.Application.DTOs;

public class CommentDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int TodoItemId { get; set; }
}

public class CreateCommentDto
{
    public string Content { get; set; } = string.Empty;
    public int TodoItemId { get; set; }
}

public class UpdateCommentDto
{
    public string Content { get; set; } = string.Empty;
} 