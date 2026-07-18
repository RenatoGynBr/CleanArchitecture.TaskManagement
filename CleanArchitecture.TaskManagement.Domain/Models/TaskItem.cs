//namespace CleanArchitecture.TaskManagement.Domain.Models
//{
//    public class TaskItem
//    {
//        public int Id { get; set; }

//        public string Title { get; set; } = string.Empty;

//        public string Description { get; set; } = string.Empty;

//        public bool IsCompleted { get; set; } = false;

//        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

//        public DateTime? DueDate { get; set; }

//        // Foreign Key - Links task to a user
//        public int UserId { get; set; }
//        public User User { get; set; } = null!;
//    }
//}

namespace CleanArchitecture.TaskManagement.Domain.Models;

public sealed class TaskItem
{
    private TaskItem()
    {
        // Required by Entity Framework Core.
    }

    public TaskItem(
        string title,
        string description,
        int userId,
        DateTime? dueDate = null)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException(
                "Task title is required.",
                nameof(title));
        }

        if (userId <= 0)
        {
            throw new ArgumentException(
                "A valid user ID is required.",
                nameof(userId));
        }

        if (dueDate.HasValue && dueDate.Value <= DateTime.UtcNow)
        {
            throw new ArgumentException(
                "Due date must be in the future.",
                nameof(dueDate));
        }

        Title = title.Trim();
        Description = description?.Trim() ?? string.Empty;
        UserId = userId;
        DueDate = dueDate;
        CreatedAt = DateTime.UtcNow;
        IsCompleted = false;
    }

    public int Id { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public bool IsCompleted { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? DueDate { get; private set; }

    public int UserId { get; private set; }

    public User User { get; private set; } = null!;

    public void Complete()
    {
        if (IsCompleted)
        {
            return;
        }

        IsCompleted = true;
    }

    public void Reopen()
    {
        IsCompleted = false;
    }

    public void ChangeDueDate(DateTime? dueDate)
    {
        if (dueDate.HasValue && dueDate.Value <= DateTime.UtcNow)
        {
            throw new ArgumentException(
                "Due date must be in the future.",
                nameof(dueDate));
        }

        DueDate = dueDate;
    }
}