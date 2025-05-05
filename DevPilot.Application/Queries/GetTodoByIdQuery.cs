using MediatR;
using DevPilot.Application.DTOs;

namespace DevPilot.Application.Queries;

public class GetTodoByIdQuery : IRequest<TodoItemDto?>
{
    public int Id { get; set; }
} 