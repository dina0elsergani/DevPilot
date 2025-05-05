using MediatR;
using AutoMapper;
using DevPilot.Application.Commands;
using DevPilot.Application.DTOs;
using DevPilot.Domain;
using DevPilot.Application.Events;
using DevPilot.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace DevPilot.Application.Handlers;

public class UpdateTodoCommandHandler : IRequestHandler<UpdateTodoCommand, TodoItemDto>
{
    private readonly IRepository<TodoItem> _repository;
    private readonly IRepository<Project> _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<UpdateTodoCommandHandler> _logger;

    public UpdateTodoCommandHandler(
        IRepository<TodoItem> repository,
        IRepository<Project> projectRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMediator mediator,
        ILogger<UpdateTodoCommandHandler> logger)
    {
        _repository = repository;
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<TodoItemDto> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Starting UpdateTodoCommandHandler with request: {@Request}", request);
            
            // Normalize empty strings to null
            if (request.Title == string.Empty)
            {
                _logger.LogWarning("Received empty title string, converting to null");
                request.Title = null;
            }
            
            if (request.Description == string.Empty)
            {
                _logger.LogWarning("Received empty description string, converting to null");
                request.Description = null;
            }
            
            var todoItem = await _repository.GetByIdAsync(request.Id);
            if (todoItem == null)
            {
                _logger.LogWarning("Todo item with ID {TodoId} not found", request.Id);
                throw new ArgumentException($"Todo item with ID {request.Id} not found");
            }

            var wasCompleted = todoItem.IsCompleted;

            if (request.ProjectId.HasValue)
            {
                _logger.LogInformation("Checking if project {ProjectId} exists", request.ProjectId);
                try
                {
                    var project = await _projectRepository.GetByIdAsync(request.ProjectId.Value);
                    if (project == null)
                    {
                        _logger.LogWarning("Project with ID {ProjectId} not found", request.ProjectId);
                        throw new ArgumentException($"Project with ID {request.ProjectId.Value} not found");
                    }
                    _logger.LogInformation("Project {ProjectId} found, updating todo item", request.ProjectId);
                    todoItem.UpdateProject(request.ProjectId.Value);
                    _logger.LogInformation("Updated project to {ProjectId}", request.ProjectId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error validating project {ProjectId}: {Message}", request.ProjectId, ex.Message);
                    throw;
                }
            }

            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                _logger.LogInformation("Updating title to {Title}", request.Title);
                try
                {
                    var title = Title.Create(request.Title);
                    todoItem.UpdateTitle(title);
                    _logger.LogInformation("Title updated successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating Title value object: {Message}", ex.Message);
                    throw;
                }
            }
            else
            {
                _logger.LogInformation("Title is null or whitespace, skipping title update");
            }

            if (request.Description != null)
            {
                _logger.LogInformation("Updating description");
                try
                {
                    var description = Description.Create(request.Description);
                    todoItem.UpdateDescription(description);
                    _logger.LogInformation("Description updated successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating Description value object: {Message}", ex.Message);
                    throw;
                }
            }
            else
            {
                _logger.LogInformation("Description is null, clearing description");
                todoItem.UpdateDescription(null);
            }

            if (request.IsCompleted.HasValue)
            {
                _logger.LogInformation("Updating completion status to {IsCompleted}", request.IsCompleted);
                if (request.IsCompleted.Value)
                    todoItem.MarkAsCompleted();
                else
                    todoItem.MarkAsIncomplete();
            }

            _repository.Update(todoItem);
            _logger.LogInformation("Todo item updated in repository");
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Changes saved to database");

            var todoDto = _mapper.Map<TodoItemDto>(todoItem);
            _logger.LogInformation("Mapped todo item to DTO: {@TodoDto}", todoDto);

            return todoDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UpdateTodoCommandHandler: {Message}", ex.Message);
            throw;
        }
    }
} 