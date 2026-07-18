//namespace CleanArchitecture.TaskManagement.Domain.Models
//{
//    public class User
//    {
//        public int Id { get; set; }

//        public string FullName { get; set; } = string.Empty;

//        public string Email { get; set; } = string.Empty;

//        public string PasswordHash { get; set; } = string.Empty;

//        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

//        // Navigation Property - One user has many tasks
//        public List<TaskItem> Tasks { get; set; } = new();
//    }
//}
namespace CleanArchitecture.TaskManagement.Domain.Models;

public sealed class User
{
    private readonly List<TaskItem> _tasks = [];

    private User()
    {
        // Required by Entity Framework Core.
    }

    public User(string fullName, string email, string passwordHash)
    {
        SetFullName(fullName);
        SetEmail(email);

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new ArgumentException(
                "Password hash is required.",
                nameof(passwordHash));
        }

        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
    }

    public int Id { get; private set; }

    public string FullName { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    public IReadOnlyCollection<TaskItem> Tasks => _tasks.AsReadOnly();

    public void SetFullName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException(
                "Full name is required.",
                nameof(fullName));
        }

        FullName = fullName.Trim();
    }

    public void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException(
                "Email is required.",
                nameof(email));
        }

        Email = email.Trim().ToLowerInvariant();
    }
}