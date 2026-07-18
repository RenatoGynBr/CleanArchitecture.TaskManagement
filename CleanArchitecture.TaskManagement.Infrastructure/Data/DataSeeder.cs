using CleanArchitecture.TaskManagement.Application.Abstractions.Persistence;
using CleanArchitecture.TaskManagement.Application.Abstractions.Security;
using CleanArchitecture.TaskManagement.Domain.Models;
using CleanArchitecture.TaskManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.TaskManagement.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var provider = scope.ServiceProvider;

        var context = provider.GetRequiredService<ApplicationDbContext>();

        // If *any* data exists, skip seeding entirely.
        if (await context.Users.AnyAsync(cancellationToken) || await context.Tasks.AnyAsync(cancellationToken))
        {
            return;
        }

        var userRepository = provider.GetRequiredService<IUserRepository>();
        var taskRepository = provider.GetRequiredService<ITaskRepository>();
        var passwordHasher = provider.GetRequiredService<IPasswordHasher>();
        var unitOfWork = provider.GetRequiredService<IUnitOfWork>();

        // Demo credentials - change as needed and avoid shipping to production
        const string demoEmail = "demo@server.com";
        const string demoPassword = "P@ssw0rd!"; // Document and change for real demos

        // Skip seeding if demo user already exists
        var existing = await userRepository.GetByEmailAsync(demoEmail, cancellationToken);
        if (existing is not null)
        {
            return;
        }

        // Create demo user
        var passwordHash = passwordHasher.Hash(demoPassword);
        var demoUser = new User(
            fullName: "Demo User",
            email: demoEmail,
            passwordHash: passwordHash);

        await userRepository.AddAsync(demoUser, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Add a couple of demo tasks for the new user
        var task1 = new TaskItem(
            title: "Welcome task",
            description: "This is a seeded demo task.",
            userId: demoUser.Id,
            dueDate: DateTime.UtcNow.AddDays(7));

        var task2 = new TaskItem(
            title: "Explore the API",
            description: "Try POST /api/tasks and PATCH /api/tasks/{id}/complete",
            userId: demoUser.Id,
            dueDate: DateTime.UtcNow.AddDays(14));

        await taskRepository.AddAsync(task1, cancellationToken);
        await taskRepository.AddAsync(task2, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}