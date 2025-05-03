using DevPilot.Domain;

namespace DevPilot.Domain.Services;

public interface IProjectDomainService
{
    Task<bool> CanUserAccessProjectAsync(string userId, int projectId);
    Task<bool> IsProjectNameUniqueAsync(string name, int? excludeProjectId = null);
    Task<int> GetProjectTodoCountAsync(int projectId);
    Task<double> GetProjectCompletionPercentageAsync(int projectId);
} 