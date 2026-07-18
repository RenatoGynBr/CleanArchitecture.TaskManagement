using CleanArchitecture.TaskManagement.Application.Common;

namespace CleanArchitecture.TaskManagement.Application.Tasks.GetTaskById;

public interface IGetTaskByIdUseCase
{
    Task<Result<TaskResponse>> ExecuteAsync(
        int taskId,
        CancellationToken cancellationToken = default);
}
