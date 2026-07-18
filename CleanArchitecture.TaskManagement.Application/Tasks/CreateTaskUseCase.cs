using CleanArchitecture.TaskManagement.Application.Abstractions.Persistence;
using CleanArchitecture.TaskManagement.Application.Common;
using CleanArchitecture.TaskManagement.Domain.Models;

namespace CleanArchitecture.TaskManagement.Application.Tasks.CreateTask;

public sealed class CreateTaskUseCase : ICreateTaskUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTaskUseCase(
        IUserRepository userRepository,
        ITaskRepository taskRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TaskResponse>> ExecuteAsync(
        CreateTaskCommand command,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(
            command.UserId,
            cancellationToken);

        if (user is null)
        {
            return Result<TaskResponse>.Failure(
                $"User {command.UserId} was not found.");
        }

        TaskItem task;

        try
        {
            task = new TaskItem(
                command.Title,
                command.Description,
                command.UserId,
                command.DueDate);
        }
        catch (ArgumentException exception)
        {
            return Result<TaskResponse>.Failure(exception.Message);
        }

        await _taskRepository.AddAsync(task, cancellationToken);
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