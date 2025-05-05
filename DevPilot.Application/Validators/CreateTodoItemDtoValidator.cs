using FluentValidation;
using DevPilot.Application.DTOs;
using DevPilot.Application.Interfaces;

namespace DevPilot.Application.Validators;

public class CreateTodoItemDtoValidator : AbstractValidator<CreateTodoItemDto>
{
    private readonly IProjectService _projectService;

    public CreateTodoItemDtoValidator(IProjectService projectService)
    {
        _projectService = projectService;

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.ProjectId)
            .GreaterThan(0).WithMessage("Project ID must be greater than 0");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required")
            .EmailAddress().WithMessage("User ID must be a valid email address");

        // Cross-field validation
        RuleFor(x => x)
            .MustAsync(async (dto, cancellation) =>
            {
                if (string.IsNullOrWhiteSpace(dto.Title) || dto.ProjectId <= 0) return true;
                
                // Business rule: Check if user has access to the project
                // This would typically check against user permissions
                return true; // TODO: Implement user access check
            }).WithMessage("User does not have access to the specified project");

        // Business rule validation
        RuleFor(x => x)
            .MustAsync(async (dto, cancellation) =>
            {
                if (dto.ProjectId <= 0) return true;
                
                // Check if project is active/accessible
                var project = await _projectService.GetByIdAsync(dto.ProjectId);
                return project != null; // Add more business rules as needed
            }).WithMessage("Cannot create todo items for inactive projects");
    }
}

public class UpdateTodoItemDtoValidator : AbstractValidator<UpdateTodoItemDto>
{
    public UpdateTodoItemDtoValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Title));
        
        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");
        
        RuleFor(x => x.ProjectId)
            .GreaterThan(0).WithMessage("ProjectId must be greater than 0")
            .When(x => x.ProjectId.HasValue);
    }
} 