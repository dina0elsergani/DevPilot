using AutoMapper;
using DevPilot.Application.DTOs;
using DevPilot.Application.Interfaces;
using DevPilot.Domain;
using DevPilot.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace DevPilot.Application.Services;

public class TodoService : ITodoService
{
    private readonly IRepository<TodoItem> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;
    private readonly IBackgroundJobService _backgroundJobService;
    private readonly EmailService _emailService;
    private readonly ILogger<TodoService> _logger;

    public TodoService(
        IRepository<TodoItem> repository, 
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        ICacheService cacheService,
        IBackgroundJobService backgroundJobService,
        EmailService emailService,
        ILogger<TodoService> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cacheService = cacheService;
        _backgroundJobService = backgroundJobService;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<IEnumerable<TodoItemDto>> GetAllAsync()
    {
        const string cacheKey = "todos:all";
        
        return await _cacheService.GetOrSetAsync(cacheKey, async () =>
        {
            _logger.LogInformation("Cache miss for {CacheKey}, fetching from database", cacheKey);
            var items = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TodoItemDto>>(items);
        }, TimeSpan.FromMinutes(5));
    }

    public async Task<TodoItemDto?> GetByIdAsync(int id)
    {
        var cacheKey = $"todos:{id}";
        
        return await _cacheService.GetOrSetAsync(cacheKey, async () =>
        {
            _logger.LogInformation("Cache miss for {CacheKey}, fetching from database", cacheKey);
            var item = await _repository.GetByIdAsync(id);
            return item == null ? null : _mapper.Map<TodoItemDto>(item);
        }, TimeSpan.FromMinutes(10));
    }

    public async Task<TodoItemDto> CreateAsync(CreateTodoItemDto createTodoItemDto)
    {
        var todoItem = new TodoItem(
            Title.Create(createTodoItemDto.Title),
            createTodoItemDto.ProjectId,
            UserId.Create(createTodoItemDto.UserId),
            createTodoItemDto.Description != null ? Description.Create(createTodoItemDto.Description) : null);
        await _repository.AddAsync(todoItem);
        await _unitOfWork.SaveChangesAsync();
        
        // Clear cache
        await _cacheService.RemoveAsync("todos:all");
        
        // Enqueue background job for welcome email
        _backgroundJobService.Enqueue<EmailService>(x => x.SendWelcomeEmail("user@example.com", "New User"));
        
        _logger.LogInformation("Created todo item {TodoId}", todoItem.Id);
        return _mapper.Map<TodoItemDto>(todoItem);
    }

    public async Task<TodoItemDto> UpdateAsync(int id, UpdateTodoItemDto updateTodoItemDto)
    {
        var todoItem = await _repository.GetByIdAsync(id);
        if (todoItem == null)
            throw new ArgumentException("Todo item not found");

        // Only update properties that are provided (not null)
        if (updateTodoItemDto.Title != null)
            todoItem.UpdateTitle(Title.Create(updateTodoItemDto.Title));
        
        if (updateTodoItemDto.Description != null)
            todoItem.UpdateDescription(Description.Create(updateTodoItemDto.Description));
        
        if (updateTodoItemDto.IsCompleted.HasValue)
        {
            if (updateTodoItemDto.IsCompleted.Value)
            {
                todoItem.MarkAsCompleted();
                // Enqueue background job for completion notification
                _backgroundJobService.Enqueue<EmailService>(x => 
                    x.SendTodoCompletionNotification(id, "user@example.com", todoItem.Title));
            }
            else
            {
                todoItem.MarkAsIncomplete();
            }
        }
        
        if (updateTodoItemDto.ProjectId.HasValue)
            todoItem.UpdateProject(updateTodoItemDto.ProjectId.Value);

        _repository.Update(todoItem);
        await _unitOfWork.SaveChangesAsync();
        
        // Clear cache
        await _cacheService.RemoveAsync($"todos:{id}");
        await _cacheService.RemoveAsync("todos:all");
        
        _logger.LogInformation("Updated todo item {TodoId}", id);
        return _mapper.Map<TodoItemDto>(todoItem);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null) return false;
        
        entity.Delete();
        _repository.Remove(entity);
        await _unitOfWork.SaveChangesAsync();
        
        // Clear cache
        await _cacheService.RemoveAsync($"todos:{id}");
        await _cacheService.RemoveAsync("todos:all");
        
        _logger.LogInformation("Deleted todo item {TodoId}", id);
        return true;
    }
} 