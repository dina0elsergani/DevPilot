using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DevPilot.Domain;
using DevPilot.Domain.ValueObjects;

namespace DevPilot.Infrastructure;

public class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure value object conversions
        modelBuilder.Entity<Comment>()
            .Property(c => c.Content)
            .HasConversion(
                content => content.Value,
                value => Content.Create(value))
            .HasMaxLength(500);

        modelBuilder.Entity<TodoItem>()
            .Property(t => t.Title)
            .HasConversion(
                title => title.Value,
                value => Title.Create(value))
            .HasMaxLength(200);

        modelBuilder.Entity<TodoItem>()
            .Property(t => t.Description)
            .HasConversion(
                description => description != null ? description.Value : null,
                value => value != null ? Description.Create(value) : null)
            .HasMaxLength(1000);

        modelBuilder.Entity<TodoItem>()
            .Property(t => t.UserId)
            .HasConversion(
                userId => userId.Value,
                value => UserId.Create(value))
            .HasMaxLength(255);

        modelBuilder.Entity<Project>()
            .Property(p => p.Name)
            .HasConversion(
                name => name.Value,
                value => Name.Create(value))
            .HasMaxLength(100);

        modelBuilder.Entity<Project>()
            .Property(p => p.Description)
            .HasConversion(
                description => description != null ? description.Value : null,
                value => value != null ? Description.Create(value) : null)
            .HasMaxLength(500);

        modelBuilder.Entity<Project>()
            .Property(p => p.UserId)
            .HasConversion(
                userId => userId.Value,
                value => UserId.Create(value))
            .HasMaxLength(255);

        // Configure relationships
        modelBuilder.Entity<Project>()
            .HasMany(p => p.TodoItems)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<TodoItem>()
            .HasMany(t => t.Comments)
            .WithOne(c => c.TodoItem)
            .HasForeignKey(c => c.TodoItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 