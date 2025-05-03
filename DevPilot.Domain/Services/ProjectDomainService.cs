using DevPilot.Domain;
using DevPilot.Domain.ValueObjects;

namespace DevPilot.Domain.Services;

public class ProjectDomainService : IProjectDomainService
{
    private readonly IRepository<Project> _projectRepository;
    private readonly IRepository<TodoItem> _todoRepository;

    public ProjectDomainService(
        IRepository<Project> projectRepository,
        IRepository<TodoItem> todoRepository)
    {
        _projectRepository = projectRepository;
        _todoRepository = todoRepository;
    }

    public async Task<bool> CanUserAccessProjectAsync(string userId, int projectId)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);
        return project != null && project.UserId.Value == userId;
    }

    public async Task<bool> IsProjectNameUniqueAsync(string name, int? excludeProjectId = null)
    {
        var projects = await _projectRepository.FindAsync(p => 
            p.Name.Value.ToLower() == name.ToLower() && 
            (!excludeProjectId.HasValue || p.Id != excludeProjectId.Value));
        
        return !projects.Any();
    }

    public async Task<int> GetProjectTodoCountAsync(int projectId)
    {
        var todos = await _todoRepository.FindAsync(t => t.ProjectId == projectId);
        return todos.Count();
    }

    public async Task<double> GetProjectCompletionPercentageAsync(int projectId)
    {
        var todos = await _todoRepository.FindAsync(t => t.ProjectId == projectId);
        var todoList = todos.ToList();
        
        if (!todoList.Any())
            return 0.0;

        var completedCount = todoList.Count(t => t.IsCompleted);
        return (double)completedCount / todoList.Count * 100;
    }
} 