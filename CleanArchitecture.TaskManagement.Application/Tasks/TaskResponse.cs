namespace CleanArchitecture.TaskManagement.Application.Tasks;

public sealed record TaskResponse(
    int Id,
    string Title,
    string Description,
    bool IsCompleted,
    DateTime CreatedAt,
    DateTime? DueDate,
    int UserId);