using DevPilot.Application.DTOs;
using DevPilot.Application.Commands;
using DevPilot.Application.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DevPilot.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class TodoItemsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<TodoItemsController> _logger;

    public TodoItemsController(IMediator mediator, ILogger<TodoItemsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllTodosQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var query = new GetTodoByIdQuery { Id = id };
        var item = await _mediator.Send(query);
        return item == null ? NotFound() : Ok(item);
    }

    /// <summary>
    /// Create a new todo item
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<TodoItemDto>> Create([FromBody] CreateTodoItemDto createTodoItemDto)
    {
        try
        {
            _logger.LogInformation("Received CreateTodoItemDto: {@CreateTodoItemDto}", createTodoItemDto);
            Console.WriteLine($"[DEBUG] Received UserId: '{createTodoItemDto.UserId}'");
            
            var command = new CreateTodoCommand
            {
                Title = createTodoItemDto.Title,
                Description = createTodoItemDto.Description,
                ProjectId = createTodoItemDto.ProjectId,
                UserId = createTodoItemDto.UserId
            };

            _logger.LogInformation("Created CreateTodoCommand: {@Command}", command);

            var todoItem = await _mediator.Send(command);
            _logger.LogInformation("Todo created successfully: {@TodoItem}", todoItem);
            
            return CreatedAtAction(nameof(Get), new { id = todoItem.Id }, todoItem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating todo: {Message}", ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Update a todo item
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<TodoItemDto>> Update(int id, [FromBody] UpdateTodoItemDto updateTodoItemDto)
    {
        _logger.LogInformation("Received update for TodoItem ID: {Id} with payload: {@Payload}", id, updateTodoItemDto);
        var command = new UpdateTodoCommand
        {
            Id = id,
            Title = updateTodoItemDto.Title,
            Description = updateTodoItemDto.Description,
            IsCompleted = updateTodoItemDto.IsCompleted,
            ProjectId = updateTodoItemDto.ProjectId
        };

        try
        {
            var todoItem = await _mediator.Send(command);
            _logger.LogInformation("Todo item updated successfully: {@TodoItem}", todoItem);
            return Ok(todoItem);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Todo item not found: {Message}", ex.Message);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating todo item: {Message}", ex.Message);
            throw;
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteTodoCommand { Id = id };
        var deleted = await _mediator.Send(command);
        return deleted ? NoContent() : NotFound();
    }
} 