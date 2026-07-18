using CleanArchitecture.TaskManagement.Application.Abstractions.Persistence;
using CleanArchitecture.TaskManagement.Application.Common;

namespace CleanArchitecture.TaskManagement.Application.Tasks.CompleteTask;

public sealed class CompleteTaskUseCase
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteTaskUseCase(
        ITaskRepository taskRepository,
        IUnitOfWork unitOfWork)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> ExecuteAsync(
        int taskId,
        CancellationToken cancellationToken = default)
    {
        var task = await _taskRepository.GetByIdAsync(
            taskId,
            cancellationToken);

        if (task is null)
        {
            return Result.Failure(
                $"Task {taskId} was not found.");
        }

        task.Complete();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}