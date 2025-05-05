using MediatR;
using DevPilot.Application.DTOs;

namespace DevPilot.Application.Commands;

public class UpdateTodoCommand : IRequest<TodoItemDto>
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool? IsCompleted { get; set; }
    public int? ProjectId { get; set; }
} 