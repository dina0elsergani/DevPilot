using DevPilot.Application.DTOs;
using FluentValidation;

namespace DevPilot.Application.Validators;

public class TodoItemDtoValidator : AbstractValidator<TodoItemDto>
{
    public TodoItemDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100);
        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
} 