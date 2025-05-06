using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DevPilot.Application.DTOs;
using DevPilot.Application.Interfaces;

namespace DevPilot.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    /// <summary>
    /// Get all comments
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetAll()
    {
        var comments = await _commentService.GetAllAsync();
        return Ok(comments);
    }

    /// <summary>
    /// Get comments by todo item ID
    /// </summary>
    [HttpGet("todo/{todoItemId}")]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetByTodoItemId(int todoItemId)
    {
        var comments = await _commentService.GetByTodoItemIdAsync(todoItemId);
        return Ok(comments);
    }

    /// <summary>
    /// Get comment by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetById(int id)
    {
        var comment = await _commentService.GetByIdAsync(id);
        if (comment == null)
            return NotFound();

        return Ok(comment);
    }

    /// <summary>
    /// Create a new comment
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<CommentDto>> Create([FromBody] CreateCommentDto createCommentDto)
    {
        var comment = await _commentService.CreateAsync(createCommentDto);
        return CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment);
    }

    /// <summary>
    /// Update comment
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<CommentDto>> Update(int id, [FromBody] UpdateCommentDto updateCommentDto)
    {
        try
        {
            var comment = await _commentService.UpdateAsync(id, updateCommentDto);
            return Ok(comment);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Delete comment
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _commentService.DeleteAsync(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }
} 