using CleanArchitecture.TaskManagement.Api.Contracts.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;

namespace CleanArchitecture.TaskManagement.Tests.Api;

public sealed class CreateTaskEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CreateTaskEndpointTests(
        WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostTask_ShouldReturnBadRequest_WhenUserDoesNotExist()
    {
        // Arrange
        var request = new CreateTaskRequest(
            "Create API",
            "Implement endpoint",
            UserId: 999,
            DueDate: DateTime.UtcNow.AddDays(2));

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/tasks",
            request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}