using MediatR;
using DevPilot.Application.Commands;
using DevPilot.Domain;
using DevPilot.Application.Events;

namespace DevPilot.Application.Handlers;

public class DeleteTodoCommandHandler : IRequestHandler<DeleteTodoCommand, bool>
{
    private readonly IRepository<TodoItem> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public DeleteTodoCommandHandler(
        IRepository<TodoItem> repository,
        IUnitOfWork unitOfWork,
        IMediator mediator)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<bool> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
    {
        var todoItem = await _repository.GetByIdAsync(request.Id);
        if (todoItem == null)
            return false;

        // Store info for domain event before deletion
        var todoId = todoItem.Id;
        var projectId = todoItem.ProjectId;

        _repository.Remove(todoItem);
        await _unitOfWork.SaveChangesAsync();

        // Publish domain event
        await _mediator.Publish(new TodoDeletedEvent
        {
            TodoId = todoId,
            ProjectId = projectId,
            DeletedAt = DateTime.UtcNow
        }, cancellationToken);

        return true;
    }
} 