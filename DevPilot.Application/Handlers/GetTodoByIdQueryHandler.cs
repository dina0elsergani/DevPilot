using MediatR;
using AutoMapper;
using DevPilot.Application.Queries;
using DevPilot.Application.DTOs;
using DevPilot.Domain;

namespace DevPilot.Application.Handlers;

public class GetTodoByIdQueryHandler : IRequestHandler<GetTodoByIdQuery, TodoItemDto?>
{
    private readonly IRepository<TodoItem> _repository;
    private readonly IMapper _mapper;

    public GetTodoByIdQueryHandler(IRepository<TodoItem> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TodoItemDto?> Handle(GetTodoByIdQuery request, CancellationToken cancellationToken)
    {
        var todo = await _repository.GetByIdAsync(request.Id);
        return todo == null ? null : _mapper.Map<TodoItemDto>(todo);
    }
} 