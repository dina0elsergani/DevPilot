using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DevPilot.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

    public ValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators,
        ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
        {
            _logger.LogWarning("Validation failed for {RequestType}: {Failures}", 
                typeof(TRequest).Name, string.Join(", ", failures.Select(f => f.ErrorMessage)));
            
            // Create a structured error response
            var errorDetails = failures.Select(f => new
            {
                Field = f.PropertyName,
                Message = f.ErrorMessage,
                Code = f.ErrorCode
            }).ToList();

            _logger.LogInformation("Validation errors: {@ErrorDetails}", errorDetails);
            
            throw new ValidationException(failures);
        }

        return await next();
    }
} 