using DevPilot.Application.DTOs;

namespace DevPilot.Application.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectDto>> GetAllAsync();
    Task<ProjectDto?> GetByIdAsync(int id);
    Task<ProjectDto> CreateAsync(CreateProjectDto createProjectDto);
    Task<ProjectDto> UpdateAsync(int id, UpdateProjectDto updateProjectDto);
    Task<bool> DeleteAsync(int id);
} 