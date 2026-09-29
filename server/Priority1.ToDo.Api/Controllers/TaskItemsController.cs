using Microsoft.AspNetCore.Mvc;
using Priority1.ToDo.Api.Models;
using Priority1.ToDo.Api.Models.Requests;
using Priority1.ToDo.Core.Services.Interfaces;

namespace Priority1.ToDo.Api.Controllers;

[ApiController]
[Route("tasks")]
public class TaskItemsController : ControllerBase
{
    private readonly ITaskItemService _taskItemService;

    public TaskItemsController(ITaskItemService taskItemService)
    {
        _taskItemService = taskItemService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TaskItemModel>>> GetAll(CancellationToken ct)
    {
        var taskItems = await _taskItemService.GetAllAsync(ct);

        return Ok(taskItems.Select(TaskItemModel.From));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskItemModel>> GetById(int id, CancellationToken ct)
    {
        var taskItem = await _taskItemService.GetByIdAsync(id, ct);

        return taskItem is null
            ? NotFound()
            : Ok(TaskItemModel.From(taskItem));
    }

    [HttpPost]
    public async Task<ActionResult<TaskItemModel>> Create([FromBody] CreateTaskItemRequest request,CancellationToken ct)
    {
        var created = await _taskItemService.CreateAsync(request.ToModel(), ct);

        return CreatedAtAction(
            nameof(GetById),
            new { id = created.Id },
            TaskItemModel.From(created));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TaskItemModel>> Update(int id,[FromBody] UpdateTaskItemRequest request,CancellationToken ct)
    {
        var updated = await _taskItemService.UpdateAsync(
            request.ToModel(id),
            ct);

        return updated is null
            ? NotFound()
            : Ok(TaskItemModel.From(updated));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var deleted = await _taskItemService.DeleteAsync(id, ct);

        return deleted
            ? NoContent()
            : NotFound();
    }
}