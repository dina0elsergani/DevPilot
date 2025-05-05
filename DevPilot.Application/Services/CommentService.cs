using AutoMapper;
using DevPilot.Application.DTOs;
using DevPilot.Application.Interfaces;
using DevPilot.Domain;
using DevPilot.Domain.ValueObjects;

namespace DevPilot.Application.Services;

public class CommentService : ICommentService
{
    private readonly IRepository<Comment> _commentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CommentService(IRepository<Comment> commentRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _commentRepository = commentRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CommentDto>> GetAllAsync()
    {
        var comments = await _commentRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<CommentDto>>(comments);
    }

    public async Task<IEnumerable<CommentDto>> GetByTodoItemIdAsync(int todoItemId)
    {
        var comments = await _commentRepository.GetAllAsync();
        var filteredComments = comments.Where(c => c.TodoItemId == todoItemId);
        return _mapper.Map<IEnumerable<CommentDto>>(filteredComments);
    }

    public async Task<CommentDto?> GetByIdAsync(int id)
    {
        var comment = await _commentRepository.GetByIdAsync(id);
        return _mapper.Map<CommentDto>(comment);
    }

    public async Task<CommentDto> CreateAsync(CreateCommentDto createCommentDto)
    {
        var comment = _mapper.Map<Comment>(createCommentDto);
        await _commentRepository.AddAsync(comment);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<CommentDto>(comment);
    }

    public async Task<CommentDto> UpdateAsync(int id, UpdateCommentDto updateCommentDto)
    {
        var comment = await _commentRepository.GetByIdAsync(id);
        if (comment == null)
            throw new ArgumentException("Comment not found");

        if (updateCommentDto.Content != null)
            comment.UpdateContent(Content.Create(updateCommentDto.Content));

        _commentRepository.Update(comment);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<CommentDto>(comment);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var comment = await _commentRepository.GetByIdAsync(id);
        if (comment == null)
            return false;

        _commentRepository.Remove(comment);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
} 