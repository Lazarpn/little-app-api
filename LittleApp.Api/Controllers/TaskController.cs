using LittleApp.Api.Authentication;
using LittleApp.Common.Exceptions;
using LittleApp.Common.Models.UserTask;
using LittleApp.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LittleApp.Api.Controllers;

[Route("api/tasks")]
[ApiController]
[Authorize(Policy = Policies.RegisteredUser)]
public class TaskController : BaseController
{
    private readonly TaskManager taskManager;

    public TaskController(TaskManager taskManager)
    {
        this.taskManager = taskManager;
    }

    /// <summary>
    /// Gets all user's tasks
    /// </summary>
    /// <returns></returns>
    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TaskModel>))]
    public async Task<IActionResult> GetTasks()
    {
        var tasks = await taskManager.GetTasks(GetCurrentUserId().Value);
        return Ok(tasks);
    }

    /// <summary>
    /// Creates a task
    /// </summary>
    /// <param name="model">UserTaskCreateModel</param>
    /// <returns>UserTask model</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskModel))]
    public async Task<IActionResult> Create([FromBody] TaskCreateModel model)
    {
        var task = await taskManager.Create(GetCurrentUserId().Value, model);
        return Ok(task);
    }

    /// <summary>
    /// Updates order for the provided user task
    /// </summary>
    /// <param name="taskId">Task identifier</param>
    /// <param name="model">Task reorder information</param>
    /// <returns></returns>
    [HttpPut("{taskId:guid}/order")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateOrder(Guid taskId, [FromBody] TaskReorderModel model)
    {
        await taskManager.UpdateOrder(GetCurrentUserId().Value, taskId, model);
        return NoContent();
    }

    /// <summary>
    /// Deletes a task with a given Id.
    /// </summary>
    /// <param name="taskId">Task identifier</param>
    /// <returns></returns>
    [HttpDelete("{taskId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute] Guid taskId)
    {
        await taskManager.Delete(GetCurrentUserId().Value, taskId);
        return NoContent();
    }
}
