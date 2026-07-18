using CleanArchitecture.TaskManagement.Api.Contracts.Tasks;
using CleanArchitecture.TaskManagement.Application.Tasks.CompleteTask;
using CleanArchitecture.TaskManagement.Application.Tasks.CreateTask;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.TaskManagement.Api.Controllers;

[ApiController]
[Route("api/tasks")]
public sealed class TasksController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateTaskRequest request,
        [FromServices] ICreateTaskUseCase useCase,
        CancellationToken cancellationToken)
    {
        var command = new CreateTaskCommand(
            request.Title,
            request.Description,
            request.UserId,
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
    public IActionResult GetById(int id)
    {
        // Implement GetTaskByIdUseCase using the same pattern.
        return Ok();
    }

    [HttpPatch("{id:int}/complete")]
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
        //[FromServices] DeleteTaskUseCase useCase,
        CancellationToken cancellationToken)
    {
        /*
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
        */
        return NoContent();
    }

}