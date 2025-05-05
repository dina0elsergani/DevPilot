using MediatR;
using DevPilot.Application.DTOs;

namespace DevPilot.Application.Queries;

public class GetAllTodosQuery : IRequest<IEnumerable<TodoItemDto>>
{
} 