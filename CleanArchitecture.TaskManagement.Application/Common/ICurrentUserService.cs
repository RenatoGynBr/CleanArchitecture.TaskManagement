namespace CleanArchitecture.TaskManagement.Application.Common;

public interface ICurrentUserService
{
    int? UserId { get; }
}
