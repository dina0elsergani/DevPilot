using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DevPilot.Infrastructure;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();

        try
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Seed test user if it doesn't exist
            var testUser = await userManager.FindByEmailAsync("dshaban696@gmail.com");
            if (testUser == null)
            {
                testUser = new IdentityUser
                {
                    UserName = "dshaban696@gmail.com",
                    Email = "dshaban696@gmail.com",
                    EmailConfirmed = true,
                    NormalizedEmail = "DSHABAN696@GMAIL.COM",
                    NormalizedUserName = "DSHABAN696@GMAIL.COM"
                };

                var result = await userManager.CreateAsync(testUser, "Dina13@");
                if (result.Succeeded)
                {
                    logger.LogInformation("Test user created successfully");
                }
                else
                {
                    logger.LogError("Failed to create test user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                logger.LogInformation("Test user already exists");
            }

            // Seed test project if none exist
            if (!context.Projects.Any())
            {
                var project = new Domain.Project(
                    Domain.ValueObjects.Name.Create("Personal Tasks"),
                    Domain.ValueObjects.UserId.Create("dshaban696@gmail.com"),
                    Domain.ValueObjects.Description.Create("My personal task management")
                );

                context.Projects.Add(project);
                await context.SaveChangesAsync();
                logger.LogInformation("Test project created successfully");
            }

            logger.LogInformation("Database seeding completed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }
} 