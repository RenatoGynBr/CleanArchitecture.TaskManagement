using CleanArchitecture.TaskManagement.Api.Contracts.Tasks;
using CleanArchitecture.TaskManagement.Application.Common;
using CleanArchitecture.TaskManagement.Application.Tasks;
using CleanArchitecture.TaskManagement.Application.Tasks.CompleteTask;
using CleanArchitecture.TaskManagement.Application.Tasks.CreateTask;
using CleanArchitecture.TaskManagement.Application.Tasks.GetTaskById;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.TaskManagement.Api.Controllers;

[ApiController]
[Route("api/tasks")]
public sealed class TasksController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create(
        [FromBody] CreateTaskRequest request,
        [FromServices] ICreateTaskUseCase useCase,
        [FromServices] ICurrentUserService currentUser,
        CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        if (userId is null)
        {
            return Unauthorized(new { error = "User is not authenticated." });
        }

        var command = new CreateTaskCommand(
            request.Title,
            request.Description,
            //request.UserId,
            userId.Value,
            request.DueDate);

        var result = await useCase.ExecuteAsync(
            command,
            cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new
            {
                error = result.Error
            });
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Value!.Id },
            result.Value);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        int id,
        [FromServices] IGetTaskByIdUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.ExecuteAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(new
            {
                error = result.Error
            });
        }

        return Ok(result.Value);
    }

    // GET api/tasks
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll(
        [FromServices] IGetAllTasks useCase,
        [FromServices] ICurrentUserService currentUser,
        CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        if (userId is null)
        {
            return Unauthorized(new { error = "User is not authenticated." });
        }

        var result = await useCase.ExecuteAsync(userId.Value, cancellationToken);

        return Ok(result.Value);
    }


    //[HttpPatch("{id:int}/complete")]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Complete(
        int id,
        [FromServices] CompleteTaskUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.ExecuteAsync(
            id,
            cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(new
            {
                error = result.Error
            });
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        [FromServices] DeleteTaskUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.ExecuteAsync(
            id,
            cancellationToken);
        if (result.IsFailure)
        {
            return NotFound(new
            {
                error = result.Error
            });
        }
        
        return NoContent();
    }

}