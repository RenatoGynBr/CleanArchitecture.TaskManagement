using CleanArchitecture.TaskManagement.Application.Tasks.CompleteTask;
using CleanArchitecture.TaskManagement.Application.Tasks.CreateTask;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.TaskManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<ICreateTaskUseCase, CreateTaskUseCase>();
        services.AddScoped<CompleteTaskUseCase>();

        return services;
    }
}