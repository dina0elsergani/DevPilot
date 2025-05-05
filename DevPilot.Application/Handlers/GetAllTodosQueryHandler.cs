using MediatR;
using AutoMapper;
using DevPilot.Application.Queries;
using DevPilot.Application.DTOs;
using DevPilot.Domain;

namespace DevPilot.Application.Handlers;

public class GetAllTodosQueryHandler : IRequestHandler<GetAllTodosQuery, IEnumerable<TodoItemDto>>
{
    private readonly IRepository<TodoItem> _repository;
    private readonly IMapper _mapper;

    public GetAllTodosQueryHandler(IRepository<TodoItem> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TodoItemDto>> Handle(GetAllTodosQuery request, CancellationToken cancellationToken)
    {
        var todos = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<TodoItemDto>>(todos);
    }
} 