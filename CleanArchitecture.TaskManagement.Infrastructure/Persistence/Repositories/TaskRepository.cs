using CleanArchitecture.TaskManagement.Application.Abstractions.Persistence;
using CleanArchitecture.TaskManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace CleanArchitecture.TaskManagement.Infrastructure.Persistence.Repositories;

public sealed class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TaskRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<TaskItem?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Tasks
            .FirstOrDefaultAsync(
                task => task.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyCollection<TaskItem>> GetByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tasks
            .AsNoTracking()
            .Where(task => task.UserId == userId)
            .OrderBy(task => task.IsCompleted)
            .ThenBy(task => task.DueDate)
            .ToListAsync(cancellationToken);
    }

    public Task AddAsync(
        TaskItem task,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Tasks
            .AddAsync(task, cancellationToken)
            .AsTask();
    }

    public void Delete(TaskItem task)
    {
        _dbContext.Tasks.Remove(task);
    }
}