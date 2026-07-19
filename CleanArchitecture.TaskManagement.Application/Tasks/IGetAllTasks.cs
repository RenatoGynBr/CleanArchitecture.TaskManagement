using CleanArchitecture.TaskManagement.Application.Common;

namespace CleanArchitecture.TaskManagement.Application.Tasks
{
    public interface IGetAllTasks
    {
        Task<Result<IReadOnlyCollection<TaskResponse>>> ExecuteAsync(
                int userId,
                CancellationToken cancellationToken = default);
    }
}
