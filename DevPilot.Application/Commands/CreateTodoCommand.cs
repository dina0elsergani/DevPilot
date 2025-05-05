using MediatR;
using DevPilot.Application.DTOs;

namespace DevPilot.Application.Commands;

public class CreateTodoCommand : IRequest<TodoItemDto>
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int ProjectId { get; set; }
    public string UserId { get; set; } = string.Empty;
} 