using CleanArchitecture.TaskManagement.Api.Contracts.Tasks;
using CleanArchitecture.TaskManagement.Application.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
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
        // Arrange - create factory that injects a fake current user with id 999
        var factoryWithUser = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped<ICurrentUserService>(_ => new FakeCurrentUserService(999));
                });
            });

        var client = factoryWithUser.CreateClient();

        var request = new CreateTaskRequest(
            "Create API",
            "Implement endpoint",
            UserId: 999, // can be left but controller uses current user instead
            DueDate: DateTime.UtcNow.AddDays(2));

        // Act
        var response = await client.PostAsJsonAsync("/api/tasks", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // helper test double
    private sealed class FakeCurrentUserService : ICurrentUserService
    {
        private readonly int? _userId;
        public FakeCurrentUserService(int? userId) => _userId = userId;
        public int? UserId => _userId;
    }
}