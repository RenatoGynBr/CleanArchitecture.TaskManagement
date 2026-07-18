using CleanArchitecture.TaskManagement.Domain.Models;

namespace CleanArchitecture.TaskManagement.Application.Abstractions.Persistence;

public interface ITaskRepository
{
    Task<TaskItem?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<TaskItem>> GetByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        TaskItem task,
        CancellationToken cancellationToken = default);

    void Delete(TaskItem task);
}