using CleanArchitecture.TaskManagement.Application.Abstractions.Persistence;
using CleanArchitecture.TaskManagement.Application.Common;

namespace CleanArchitecture.TaskManagement.Application.Tasks.GetTaskById;

public sealed class GetTaskByIdUseCase : IGetTaskByIdUseCase
{
    private readonly ITaskRepository _taskRepository;

    public GetTaskByIdUseCase(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<Result<TaskResponse>> ExecuteAsync(
        int taskId,
        CancellationToken cancellationToken = default)
    {
        var task = await _taskRepository.GetByIdAsync(
            taskId,
            cancellationToken);

        if (task is null)
        {
            return Result<TaskResponse>.Failure(
                $"Task {taskId} was not found.");
        }

        var response = new TaskResponse(
            task.Id,
            task.Title,
            task.Description,
            task.IsCompleted,
            task.CreatedAt,
            task.DueDate,
            task.UserId);

        return Result<TaskResponse>.Success(response);
    }
}
