using CleanArchitecture.TaskManagement.Application.Common;
using CleanArchitecture.TaskManagement.Application.Tasks.CreateTask;

namespace CleanArchitecture.TaskManagement.Application.Tasks
{
    public sealed class DeleteTaskUseCase : IDeleteTaskUseCase
    {
        public Task<Result<TaskResponse>> ExecuteAsync(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
