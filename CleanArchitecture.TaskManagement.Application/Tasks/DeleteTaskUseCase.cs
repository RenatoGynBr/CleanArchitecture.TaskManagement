using CleanArchitecture.TaskManagement.Application.Abstractions.Persistence;
using CleanArchitecture.TaskManagement.Application.Common;
using CleanArchitecture.TaskManagement.Application.Tasks.CreateTask;

namespace CleanArchitecture.TaskManagement.Application.Tasks;

public sealed class DeleteTaskUseCase : IDeleteTaskUseCase
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTaskUseCase(
        ITaskRepository taskRepository,
        IUnitOfWork unitOfWork)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TaskResponse>> ExecuteAsync(int Id, CancellationToken cancellationToken = default)
    {
        var task = await _taskRepository.GetByIdAsync(Id, cancellationToken);

        if (task is null)
        {
            return Result<TaskResponse>.Failure(
                $"Task {Id} was not found.");
        }

        _taskRepository.Delete(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

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