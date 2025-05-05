using MediatR;
using AutoMapper;
using DevPilot.Application.Commands;
using DevPilot.Application.DTOs;
using DevPilot.Domain;
using DevPilot.Application.Events;
using DevPilot.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace DevPilot.Application.Handlers;

public class CreateTodoCommandHandler : IRequestHandler<CreateTodoCommand, TodoItemDto>
{
    private readonly IRepository<TodoItem> _repository;
    private readonly IRepository<Project> _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<CreateTodoCommandHandler> _logger;

    public CreateTodoCommandHandler(
        IRepository<TodoItem> repository,
        IRepository<Project> projectRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMediator mediator,
        ILogger<CreateTodoCommandHandler> logger)
    {
        _repository = repository;
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<TodoItemDto> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Creating todo item for project {ProjectId}", request.ProjectId);

            // Domain validation - ensure project exists
            var project = await _projectRepository.GetByIdAsync(request.ProjectId);
            if (project == null)
            {
                _logger.LogError("Project with ID {ProjectId} not found", request.ProjectId);
                throw new ArgumentException($"Project with ID {request.ProjectId} not found");
            }

            // Create value objects
            var title = Title.Create(request.Title);
            var userId = UserId.Create(request.UserId);
            var description = request.Description != null ? Description.Create(request.Description) : null;

            // Domain logic - create todo with business rules
            var todoItem = new TodoItem(title, request.ProjectId, userId, description);

            await _repository.AddAsync(todoItem);
            await _unitOfWork.SaveChangesAsync();

            // Add domain event after the entity has an ID
            todoItem.AddDomainEvent(new DevPilot.Domain.Events.TodoCreatedEvent(todoItem.Id, title.Value, request.ProjectId, userId.Value));

            var todoDto = _mapper.Map<TodoItemDto>(todoItem);

            // Publish domain event via MediatR
            await _mediator.Publish(new TodoCreatedEvent
            {
                TodoId = todoItem.Id,
                ProjectId = request.ProjectId,
                CreatedAt = DateTime.UtcNow
            }, cancellationToken);

            _logger.LogInformation("Todo item {TodoId} created successfully", todoItem.Id);
            return todoDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating todo item: {Message}", ex.Message);
            throw;
        }
    }
} 