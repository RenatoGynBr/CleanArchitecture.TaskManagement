using CleanArchitecture.TaskManagement.Application.Abstractions.Persistence;
using CleanArchitecture.TaskManagement.Application.Common;
using System.Threading.Tasks;

namespace CleanArchitecture.TaskManagement.Application.Tasks
{
    public sealed class GetAllTasks : IGetAllTasks
    {
        private readonly ITaskRepository _taskRepository;

        public GetAllTasks(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Result<IReadOnlyCollection<TaskResponse>>> ExecuteAsync(int userId, CancellationToken cancellationToken = default)
        {
            var list = await _taskRepository.GetByUserIdAsync(userId, cancellationToken);

            if (list is null || !list.Any())
            {
                return Result<IReadOnlyCollection<TaskResponse>>.Failure(
                    $"No tasks found for user {userId}.");
            }

            var responseList = list.Select(task => new TaskResponse(
                task.Id,
                task.Title,
                task.Description,
                task.IsCompleted,
                task.CreatedAt,
                task.DueDate,
                task.UserId)).ToList();

            return Result<IReadOnlyCollection<TaskResponse>>.Success(responseList);
        }
    }
}
