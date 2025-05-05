using FluentValidation;
using DevPilot.Application.DTOs;
using DevPilot.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace DevPilot.Application.Validators;

public class CreateProjectDtoValidator : AbstractValidator<CreateProjectDto>
{
    private readonly IProjectService _projectService;

    public CreateProjectDtoValidator(IProjectService projectService)
    {
        _projectService = projectService;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required")
            .MaximumLength(100).WithMessage("Project name cannot exceed 100 characters")
            .MustAsync(async (name, cancellation) =>
            {
                if (string.IsNullOrWhiteSpace(name)) return false;
                
                // Check for duplicate project names for the same user
                // This would typically check against user's existing projects
                return await BeUniqueName(name, cancellation);
            }).WithMessage("A project with this name already exists");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required")
            .EmailAddress().WithMessage("User ID must be a valid email address");

        // Business rule validation
        RuleFor(x => x)
            .MustAsync(async (dto, cancellation) =>
            {
                if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.UserId)) return true;
                
                // Check if user can create more projects (e.g., subscription limits)
                // This would typically check against user's subscription/plan
                return await BeWithinSubscriptionLimit(cancellation);
            }).WithMessage("User has reached the maximum number of projects allowed");

        // Cross-field validation
        RuleFor(x => x)
            .MustAsync(async (dto, cancellation) =>
            {
                if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.UserId)) return true;
                
                // Validate project name format (no special characters, etc.)
                var name = dto.Name.Trim();
                if (name.Length < 3) return false;
                
                // Check for reserved names
                var reservedNames = new[] { "admin", "system", "root", "default" };
                return !reservedNames.Contains(name.ToLowerInvariant());
            }).WithMessage("Project name contains reserved words or is too short");
    }

    private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        // In a real application, you would check against the database
        // For now, we'll assume names are unique
        return true;
    }

    private async Task<bool> BeWithinSubscriptionLimit(CancellationToken cancellationToken)
    {
        // In a real application, you would check the user's subscription limits
        // For now, we'll assume unlimited projects
        return true;
    }
} 