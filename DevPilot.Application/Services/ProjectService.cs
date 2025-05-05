using AutoMapper;
using DevPilot.Application.DTOs;
using DevPilot.Application.Interfaces;
using DevPilot.Domain;
using DevPilot.Domain.ValueObjects;

namespace DevPilot.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IRepository<Project> _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProjectService(IRepository<Project> projectRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProjectDto>> GetAllAsync()
    {
        var projects = await _projectRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ProjectDto>>(projects);
    }

    public async Task<ProjectDto?> GetByIdAsync(int id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        return _mapper.Map<ProjectDto>(project);
    }

    public async Task<ProjectDto> CreateAsync(CreateProjectDto createProjectDto)
    {
        var project = _mapper.Map<Project>(createProjectDto);
        await _projectRepository.AddAsync(project);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<ProjectDto>(project);
    }

    public async Task<ProjectDto> UpdateAsync(int id, UpdateProjectDto updateProjectDto)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null)
            throw new ArgumentException("Project not found");

        if (updateProjectDto.Name != null)
            project.UpdateName(Name.Create(updateProjectDto.Name));
        
        if (updateProjectDto.Description != null)
            project.UpdateDescription(Description.Create(updateProjectDto.Description));

        _projectRepository.Update(project);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<ProjectDto>(project);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null)
            return false;

        _projectRepository.Remove(project);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
} 