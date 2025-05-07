using AutoMapper;
using DevPilot.Application.DTOs;
using DevPilot.Application.Mapping;
using DevPilot.Application.Services;
using DevPilot.Domain;
using DevPilot.Application.Interfaces;
using DevPilot.Domain.ValueObjects;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;

public class TodoServiceTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IRepository<TodoItem>> _repoMock = new();
    private readonly Mock<IUnitOfWork> _uowMock = new();
    private readonly Mock<ICacheService> _cacheMock = new();
    private readonly Mock<IBackgroundJobService> _jobMock = new();
    private readonly EmailService _emailService;
    private readonly Mock<ILogger<TodoService>> _loggerMock = new();
    private readonly TodoService _service;

    public TodoServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
        _emailService = new EmailService(new Mock<ILogger<EmailService>>().Object);
        _service = new TodoService(_repoMock.Object, _uowMock.Object, _mapper, _cacheMock.Object, _jobMock.Object, _emailService, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDtos()
    {
        var todoItem = new TodoItem(Title.Create("Test Todo"), 1, UserId.Create("test@example.com"), Description.Create("Test description"));
        var todoItems = new[] { todoItem };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(todoItems);
        _cacheMock.Setup(c => c.GetOrSetAsync(
            It.IsAny<string>(),
            It.IsAny<Func<Task<IEnumerable<TodoItemDto>>>>(),
            It.IsAny<TimeSpan?>()
        )).Returns<string, Func<Task<IEnumerable<TodoItemDto>>>, TimeSpan?>((key, factory, expiration) => factory());

        var result = await _service.GetAllAsync();
        Assert.Single(result);
        Assert.Equal("Test Todo", result.First().Title);
    }

    [Fact]
    public async Task CreateAsync_SavesAndReturnsDto()
    {
        var dto = new CreateTodoItemDto { Title = "New", ProjectId = 1, UserId = "test@example.com", Description = "Test description" };
        var todoItem = new TodoItem(Title.Create("New"), 1, UserId.Create("test@example.com"), Description.Create("Test description"));
        _repoMock.Setup(r => r.AddAsync(It.IsAny<TodoItem>())).ReturnsAsync(todoItem);
        _uowMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
        var result = await _service.CreateAsync(dto);
        Assert.Equal("New", result.Title);
    }
} 