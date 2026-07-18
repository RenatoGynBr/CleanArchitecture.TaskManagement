using CleanArchitecture.TaskManagement.Domain.Models;
using FluentAssertions;

namespace CleanArchitecture.TaskManagement.Tests.Domain;

public sealed class TaskItemTests
{
    [Fact]
    public void Constructor_ShouldCreateIncompleteTask_WhenDataIsValid()
    {
        // Arrange
        var dueDate = DateTime.UtcNow.AddDays(2);

        // Act
        var task = new TaskItem(
            "Create API",
            "Implement the task endpoint",
            userId: 1,
            dueDate);

        // Assert
        task.Title.Should().Be("Create API");
        task.UserId.Should().Be(1);
        task.DueDate.Should().Be(dueDate);
        task.IsCompleted.Should().BeFalse();
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenTitleIsEmpty()
    {
        // Act
        var action = () => new TaskItem(
            string.Empty,
            "Description",
            userId: 1);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("*title*");
    }

    [Fact]
    public void Complete_ShouldMarkTaskAsCompleted()
    {
        // Arrange
        var task = new TaskItem(
            "Create tests",
            "Implement unit tests",
            userId: 1);

        // Act
        task.Complete();

        // Assert
        task.IsCompleted.Should().BeTrue();
    }
}