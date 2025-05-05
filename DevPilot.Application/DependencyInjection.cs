using AutoMapper;
using DevPilot.Application.Mapping;
using DevPilot.Application.Services;
using DevPilot.Application.Validators;
using DevPilot.Application.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using DevPilot.Application.Behaviors;
using DevPilot.Domain;

namespace DevPilot.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITodoService, TodoService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<ICacheService, MemoryCacheService>();
        services.AddScoped<IBackgroundJobService, BackgroundJobService>();
        services.AddScoped<EmailService>();
        services.AddAutoMapper(typeof(MappingProfile));
        
        // MediatR and FluentValidation are registered in Program.cs
        
        services.AddMediatR(typeof(DependencyInjection).Assembly);
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        
        return services;
    }
} 