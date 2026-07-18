using CleanArchitecture.TaskManagement.Application.Common;

namespace CleanArchitecture.TaskManagement.Application.Tasks.CreateTask;

public interface ICreateTaskUseCase
{
    Task<Result<TaskResponse>> ExecuteAsync(
        CreateTaskCommand command,
        CancellationToken cancellationToken = default);
}