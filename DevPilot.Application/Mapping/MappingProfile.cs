using AutoMapper;
using DevPilot.Domain;
using DevPilot.Application.DTOs;
using DevPilot.Domain.ValueObjects;
using System.Collections.Generic;

namespace DevPilot.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // TodoItem mappings
        CreateMap<TodoItem, TodoItemDto>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.Value))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description != null ? src.Description.Value : null))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.Value))
            .ForMember(dest => dest.Project, opt => opt.MapFrom(src => src.Project != null ? new ProjectDto
            {
                Id = src.Project.Id,
                Name = src.Project.Name.Value,
                Description = src.Project.Description != null ? src.Project.Description.Value : null,
                UserId = src.Project.UserId.Value,
                CreatedAt = src.Project.CreatedAt,
                TodoItems = new List<TodoItemDto>() // Empty list to prevent cycle
            } : null));
        
        // CreateMap<UpdateTodoItemDto, TodoItem>(); // Removed - not needed and causes conflicts

        // Project mappings
        CreateMap<Project, ProjectDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description != null ? src.Description.Value : null))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.Value))
            .ForMember(dest => dest.TodoItems, opt => opt.MapFrom(src => src.TodoItems != null ? src.TodoItems.Select(todo => new TodoItemDto
            {
                Id = todo.Id,
                Title = todo.Title.Value,
                Description = todo.Description != null ? todo.Description.Value : null,
                IsCompleted = todo.IsCompleted,
                CreatedAt = todo.CreatedAt,
                ProjectId = todo.ProjectId,
                UserId = todo.UserId.Value,
                Project = null, // Prevent cycle by not including Project
                Comments = new List<CommentDto>() // Empty list to prevent cycle
            }).ToList() : new List<TodoItemDto>()));
        
        CreateMap<CreateProjectDto, Project>()
            .ConstructUsing(dto => new Project(
                Name.Create(dto.Name),
                UserId.Create(dto.UserId),
                dto.Description != null ? Description.Create(dto.Description) : null));
        
        CreateMap<UpdateProjectDto, Project>();

        // Comment mappings
        CreateMap<Comment, CommentDto>()
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content.Value));
        
        CreateMap<CreateCommentDto, Comment>()
            .ConstructUsing(dto => new Comment(
                Content.Create(dto.Content),
                dto.TodoItemId));
        
        CreateMap<UpdateCommentDto, Comment>();
    }
} 