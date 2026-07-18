namespace CleanArchitecture.TaskManagement.Api.Contracts.Tasks;

public sealed record CreateTaskRequest(
    string Title,
    string Description,
    int UserId,
    DateTime? DueDate);