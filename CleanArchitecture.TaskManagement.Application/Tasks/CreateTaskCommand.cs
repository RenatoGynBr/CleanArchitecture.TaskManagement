namespace CleanArchitecture.TaskManagement.Application.Tasks.CreateTask;

public sealed record CreateTaskCommand(
    string Title,
    string Description,
    int UserId,
    DateTime? DueDate);