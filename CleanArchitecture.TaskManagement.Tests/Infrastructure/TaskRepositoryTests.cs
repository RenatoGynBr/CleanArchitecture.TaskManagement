using CleanArchitecture.TaskManagement.Application.Abstractions.Persistence;
using CleanArchitecture.TaskManagement.Domain.Models;
using CleanArchitecture.TaskManagement.Infrastructure.Persistence;
using CleanArchitecture.TaskManagement.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;

namespace CleanArchitecture.TaskManagement.Tests.Infrastructure;

public sealed class TaskRepositoryTests : IAsyncDisposable
{
    private readonly SqliteConnection _connection;
    private readonly ApplicationDbContext _dbContext;

    public TaskRepositoryTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _dbContext.Database.EnsureCreated();
    }

    [Fact]
    public async Task GetByUserIdAsync_ShouldReturnOnlyUserTasks()
    {
        // Arrange
        var firstUser = new User(
            "First User",
            "first@example.com",
            "hash");

        var secondUser = new User(
            "Second User",
            "second@example.com",
            "hash");

        _dbContext.Users.AddRange(firstUser, secondUser);
        await _dbContext.SaveChangesAsync();

        _dbContext.Tasks.AddRange(
            new TaskItem("Task 1", "Description", firstUser.Id),
            new TaskItem("Task 2", "Description", firstUser.Id),
            new TaskItem("Task 3", "Description", secondUser.Id));

        await _dbContext.SaveChangesAsync();

        var repository = new TaskRepository(_dbContext);

        // Act
        var tasks = await repository.GetByUserIdAsync(firstUser.Id);

        // Assert
        tasks.Should().HaveCount(2);
        tasks.Should().OnlyContain(task => task.UserId == firstUser.Id);
    }

    public async ValueTask DisposeAsync()
    {
        await _dbContext.DisposeAsync();
        await _connection.DisposeAsync();
    }
}