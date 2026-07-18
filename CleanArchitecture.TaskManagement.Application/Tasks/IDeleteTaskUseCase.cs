using CleanArchitecture.TaskManagement.Application.Common;

namespace CleanArchitecture.TaskManagement.Application.Tasks.CreateTask;

public interface IDeleteTaskUseCase
{
    Task<Result<TaskResponse>> ExecuteAsync(
        int Id,
        CancellationToken cancellationToken = default);
}
