using DevPilot.Api.Controllers;
using DevPilot.Application.DTOs;
using DevPilot.Application.Commands;
using DevPilot.Application.Queries;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MediatR;
using Xunit;
using Microsoft.Extensions.Logging;

public class TodoItemsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ILogger<TodoItemsController>> _loggerMock;
    private readonly TodoItemsController _controller;

    public TodoItemsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new Mock<ILogger<TodoItemsController>>();
        _controller = new TodoItemsController(_mediatorMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithItems()
    {
        var expectedItems = new[] { new TodoItemDto { Id = 1, Title = "Test" } };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllTodosQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedItems);
        
        var result = await _controller.GetAll();
        var ok = Assert.IsType<OkObjectResult>(result);
        var items = Assert.IsAssignableFrom<IEnumerable<TodoItemDto>>(ok.Value);
        Assert.Single(items);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction()
    {
        var createDto = new CreateTodoItemDto { Title = "Test", ProjectId = 1 };
        var resultDto = new TodoItemDto { Id = 1, Title = "Test", ProjectId = 1 };
        
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateTodoCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultDto);
        
        var result = await _controller.Create(createDto);
        var actionResult = Assert.IsType<ActionResult<TodoItemDto>>(result);
        var created = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        Assert.Equal(resultDto, created.Value);
    }
} 