using CleanArchitecture.TaskManagement.Application.Tasks;
using CleanArchitecture.TaskManagement.Application.Tasks.CompleteTask;
using CleanArchitecture.TaskManagement.Application.Tasks.CreateTask;
using CleanArchitecture.TaskManagement.Application.Tasks.GetTaskById;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.TaskManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<ICreateTaskUseCase, CreateTaskUseCase>();
        services.AddScoped<IGetTaskByIdUseCase, GetTaskByIdUseCase>();
        services.AddScoped<IDeleteTaskUseCase, DeleteTaskUseCase>();
        services.AddScoped<DeleteTaskUseCase>();
        services.AddScoped<CompleteTaskUseCase>();

        return services;
    }
}