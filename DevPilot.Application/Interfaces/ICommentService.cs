using DevPilot.Application.DTOs;

namespace DevPilot.Application.Interfaces;

public interface ICommentService
{
    Task<IEnumerable<CommentDto>> GetAllAsync();
    Task<IEnumerable<CommentDto>> GetByTodoItemIdAsync(int todoItemId);
    Task<CommentDto?> GetByIdAsync(int id);
    Task<CommentDto> CreateAsync(CreateCommentDto createCommentDto);
    Task<CommentDto> UpdateAsync(int id, UpdateCommentDto updateCommentDto);
    Task<bool> DeleteAsync(int id);
} 