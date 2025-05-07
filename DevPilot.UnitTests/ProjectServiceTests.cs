using AutoMapper;
using DevPilot.Application.DTOs;
using DevPilot.Application.Mapping;
using DevPilot.Application.Services;
using DevPilot.Domain;
using DevPilot.Application.Interfaces;
using DevPilot.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using Xunit;

namespace DevPilot.UnitTests;

public class ProjectServiceTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IRepository<Project>> _repoMock = new();
    private readonly Mock<IUnitOfWork> _uowMock = new();
    private readonly ProjectService _service;

    public ProjectServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
        _service = new ProjectService(_repoMock.Object, _uowMock.Object, _mapper);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDtos()
    {
        // Arrange
        var projects = new[] 
        { 
            new Project(Name.Create("Project 1"), UserId.Create("test@example.com"), Description.Create("Description 1")),
            new Project(Name.Create("Project 2"), UserId.Create("test@example.com"), Description.Create("Description 2"))
        };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(projects);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.First().Name.Should().Be("Project 1");
        result.Last().Name.Should().Be("Project 2");
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsProject()
    {
        // Arrange
        var project = new Project(Name.Create("Test Project"), UserId.Create("test@example.com"), null);
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(project);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Test Project");
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Project?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_WithValidDto_CreatesAndReturnsProject()
    {
        // Arrange
        var createDto = new CreateProjectDto { Name = "New Project", Description = "New Description", UserId = "test@example.com" };
        var project = new Project(Name.Create("New Project"), UserId.Create("test@example.com"), Description.Create("New Description"));
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Project>())).ReturnsAsync(project);
        _uowMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("New Project");
        result.Description.Should().Be("New Description");
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Project>()), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithValidId_UpdatesAndReturnsProject()
    {
        // Arrange
        var existingProject = new Project(Name.Create("Old Name"), UserId.Create("test@example.com"), Description.Create("Old Description"));
        var updateDto = new UpdateProjectDto { Name = "Updated Name", Description = "Updated Description" };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingProject);
        _uowMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _service.UpdateAsync(1, updateDto);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Updated Name");
        result.Description.Should().Be("Updated Description");
        _repoMock.Verify(r => r.Update(It.IsAny<Project>()), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidId_ThrowsArgumentException()
    {
        // Arrange
        var updateDto = new UpdateProjectDto { Name = "Updated Name" };
        _repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Project?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateAsync(999, updateDto));
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_DeletesProject()
    {
        // Arrange
        var project = new Project(Name.Create("Test Project"), UserId.Create("test@example.com"), null);
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(project);
        _uowMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        result.Should().BeTrue();
        _repoMock.Verify(r => r.Remove(project), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithInvalidId_ReturnsFalse()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Project?)null);

        // Act
        var result = await _service.DeleteAsync(999);

        // Assert
        result.Should().BeFalse();
    }
} 