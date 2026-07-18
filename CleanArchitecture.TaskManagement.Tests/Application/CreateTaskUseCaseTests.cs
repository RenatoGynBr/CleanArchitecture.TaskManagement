using CleanArchitecture.TaskManagement.Application.Abstractions.Persistence;
using CleanArchitecture.TaskManagement.Application.Tasks.CreateTask;
using CleanArchitecture.TaskManagement.Domain.Models;
using FluentAssertions;
using Moq;

namespace CleanArchitecture.TaskManagement.Tests.Application;

public sealed class CreateTaskUseCaseTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<ITaskRepository> _taskRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    [Fact]
    public async Task ExecuteAsync_ShouldCreateTask_WhenUserExists()
    {
        // Arrange
        var user = new User(
            "Renato Silva",
            "renato@example.com",
            "password-hash");

        _userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                1,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        TaskItem? capturedTask = null;

        _taskRepositoryMock
            .Setup(repository => repository.AddAsync(
                It.IsAny<TaskItem>(),
                It.IsAny<CancellationToken>()))
            .Callback<TaskItem, CancellationToken>(
                (task, _) => capturedTask = task)
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var useCase = new CreateTaskUseCase(
            _userRepositoryMock.Object,
            _taskRepositoryMock.Object,
            _unitOfWorkMock.Object);

        var command = new CreateTaskCommand(
            "Create API",
            "Create the task controller",
            UserId: 1,
            DueDate: DateTime.UtcNow.AddDays(2));

        // Act
        var result = await useCase.ExecuteAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        capturedTask.Should().NotBeNull();
        capturedTask!.Title.Should().Be("Create API");
        capturedTask.UserId.Should().Be(1);

        _taskRepositoryMock.Verify(
            repository => repository.AddAsync(
                It.IsAny<TaskItem>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenUserDoesNotExist()
    {
        // Arrange
        _userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                999,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var useCase = new CreateTaskUseCase(
            _userRepositoryMock.Object,
            _taskRepositoryMock.Object,
            _unitOfWorkMock.Object);

        var command = new CreateTaskCommand(
            "Create API",
            "Create the task controller",
            UserId: 999,
            DueDate: null);

        // Act
        var result = await useCase.ExecuteAsync(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain("not found");

        _taskRepositoryMock.Verify(
            repository => repository.AddAsync(
                It.IsAny<TaskItem>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}