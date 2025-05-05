using DevPilot.Application.DTOs;

namespace DevPilot.Application.Interfaces;

/// <summary>
/// Service interface for managing TodoItems.
/// </summary>
public interface ITodoService
{
    Task<IEnumerable<TodoItemDto>> GetAllAsync();
    Task<TodoItemDto?> GetByIdAsync(int id);
    Task<TodoItemDto> CreateAsync(CreateTodoItemDto createTodoItemDto);
    Task<TodoItemDto> UpdateAsync(int id, UpdateTodoItemDto updateTodoItemDto);
    Task<bool> DeleteAsync(int id);
} 