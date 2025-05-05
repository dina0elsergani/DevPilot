using FluentValidation;
using DevPilot.Application.DTOs;

namespace DevPilot.Application.Validators;

public class CreateCommentDtoValidator : AbstractValidator<CreateCommentDto>
{
    public CreateCommentDtoValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Comment content is required")
            .MaximumLength(1000).WithMessage("Comment content cannot exceed 1000 characters");
        
        RuleFor(x => x.TodoItemId)
            .GreaterThan(0).WithMessage("TodoItemId must be greater than 0");
    }
}

public class UpdateCommentDtoValidator : AbstractValidator<UpdateCommentDto>
{
    public UpdateCommentDtoValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Comment content is required")
            .MaximumLength(1000).WithMessage("Comment content cannot exceed 1000 characters");
    }
} 